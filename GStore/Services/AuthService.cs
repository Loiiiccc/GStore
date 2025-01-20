using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Azure.Core;
using GStore.Data;
using GStore.Models;
using GStore.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GStore.Services
{
    public class AuthService(GStoreDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDTO request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }

            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.Role = "Client";
            if (await context.Users.CountAsync() == 0)
                user.ClientCode = 1000;
            user.ClientCode = context.Users.Max(u => u.ClientCode) + 1;

            context.Users.Add(user);
            await context.SaveChangesAsync();


            return user;
        }

        public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
        {
            var user = context.Users.FirstOrDefault(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }


            return await CreateTokenresponse(user);
        }

        private async Task<TokenResponseDTO> CreateTokenresponse(User? user)
        {
            return new TokenResponseDTO
            {
                AccessToken = CreateToken(user),
                RefreshToken = await CreateRefreshTokenAsync(user)
            };
        }

        public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            if (user is null)
                return null;

            return await CreateTokenresponse(user);
        }


        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, "Admin")};


            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescription);
            return jwt;
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        private async Task<string> CreateRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }
            return user;
        }

        public Task<string> GetUserRoleAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task SetTokenCookieAsync(TokenResponseDTO tokenResponse, HttpContext httpContext)
        {
            httpContext.Response.Cookies.Append(
                "AccessToken",
                tokenResponse.AccessToken,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                });
            httpContext.Response.Cookies.Append(
                "RefreshToken",
                tokenResponse.RefreshToken,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                });
            return Task.CompletedTask;
        }

        public async Task<User?> GetUserDataByUsernameAsync(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null )
            {
                return null;
            }
            return user;
        }
    }
}

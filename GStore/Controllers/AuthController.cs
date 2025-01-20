using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GStore.Models;
using GStore.Models.DTO;
using GStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new User();

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login(UserDTO request)
        {
            var result = await authService.LoginAsync(request);
            if (result != null)
            {
                await authService.SetTokenCookieAsync(result, HttpContext);

                return Ok(result);

            }
            return BadRequest("Username or password incorrect");

        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("User already exists");
            }
            return Ok(user);
        }


        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
        //{
        //    var result = await authService.RefreshTokenAsync(request);
        //    if (result is null || result.AccessToken is null || result.RefreshToken is null)
        //        return Unauthorized("Unvalid refresh token");

        //    return Ok(result);
        //}

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken(string username, RefreshTokenRequestDTO requestToken)
        {
            HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken);
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            var user = await authService.GetUserDataByUsernameAsync(username);
            var request = new RefreshTokenRequestDTO {UserId = user.Id, RefreshToken = refreshToken };
            var result = await authService.RefreshTokenAsync(request);
            if (result is null /*|| result.AccessToken is null */|| result.RefreshToken is null)
                return Unauthorized("Unvalid refresh token");

            await authService.SetTokenCookieAsync(result, HttpContext);
            return Ok(result);
        }


        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are authenticated and admin!");
        }
    }
}

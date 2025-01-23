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
        public async Task<ActionResult<User>> Login(UserDTO request)
        {
            var response = new LoginResponse();
            var result = await authService.LoginAsync(request);
            if (result != null)
            {
                response.IsLogged = true;
                response.Token = result.AccessToken;
                return Ok(response);

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


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
        {
            var result = await authService.RefreshTokenAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Unvalid refresh token");

            return Ok(result);
        }

        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO requestToken)
        //{

        //    var result = await authService.RefreshTokenAsync(requestToken);
        //    if (result is null /*|| result.AccessToken is null */|| result.RefreshToken is null)
        //        return Unauthorized("Unvalid refresh token");

        //    await authService.SetTokenCookieAsync(result, HttpContext);
        //    return Ok(result);
        //}


        //public async Task<IActionResult> Logout()
        // {
        //     await authService.LogoutAsync();
        //     return NoContent();
        // }


        [HttpGet("user/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await authService.GetUserDataByUsernameAsync(username);
            return Ok(user);
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

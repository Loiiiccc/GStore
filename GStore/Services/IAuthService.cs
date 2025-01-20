using GStore.Models;
using GStore.Models.DTO;

namespace GStore.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO user);
        Task<TokenResponseDTO?> LoginAsync(UserDTO user);
        Task<User?> GetUserDataByUsernameAsync(string username);
        Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
        Task SetTokenCookieAsync(TokenResponseDTO tokenResponse, HttpContext httpContext);
        Task<string> GetUserRoleAsync(Guid userId);

    }
}

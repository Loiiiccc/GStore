using GStore.Models;
using GStore.Models.DTO;

namespace GStore.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO user);
        Task<TokenResponseDTO?> LoginAsync(UserDTO user);
        Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);

    }
}

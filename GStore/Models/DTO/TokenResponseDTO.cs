namespace GStore.Models.DTO
{
    public class TokenResponseDTO
    {
        public required string AccessToken { get; set; }
        public string? Username { get; set; }
        public required string RefreshToken { get; set; }
    }
}

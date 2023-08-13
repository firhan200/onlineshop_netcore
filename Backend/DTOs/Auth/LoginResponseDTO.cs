namespace DTOs.Auth
{
    public class LoginResponseDTO{
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
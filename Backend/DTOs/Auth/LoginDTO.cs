using System.ComponentModel;

namespace DTOs.Auth
{
    public class LoginDTO {
        [DefaultValue("john@email.com")]
        public string Username { get; set; } = string.Empty;
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}
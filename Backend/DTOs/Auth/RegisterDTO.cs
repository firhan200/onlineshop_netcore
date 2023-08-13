using System.ComponentModel;

namespace DTOs.Auth
{
    public class RegisterDTO {
        [DefaultValue("John Doe")]
        public string FullName { get; set; } = string.Empty;
        [DefaultValue("john@email.com")]
        public string EmailAddress { get; set; } = string.Empty;
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}
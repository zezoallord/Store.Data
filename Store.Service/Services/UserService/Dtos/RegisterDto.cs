using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.UserService.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}\"':;?/&<>])(?!.*\\s).*$",
         ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non-alphanumeric character, and must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}

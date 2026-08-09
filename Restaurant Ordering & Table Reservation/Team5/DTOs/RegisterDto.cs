using System.ComponentModel.DataAnnotations;

namespace Team5.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Role { get; set; } = "Customer";
    }
}
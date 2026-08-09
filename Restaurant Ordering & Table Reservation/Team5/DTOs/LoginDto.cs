using System.ComponentModel.DataAnnotations;

namespace Team5.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.Models
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set;}
        [Required]
        [MinLength(5)]
        public string Password { get; set;}
    }
}

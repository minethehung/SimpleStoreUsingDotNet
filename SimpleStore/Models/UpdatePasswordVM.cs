using System.ComponentModel.DataAnnotations;

namespace SimpleStore.Models
{
    public class UpdatePasswordVM
    {
        [Required]
        [MinLength(5)]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(5)]
        public string NewPassword { get; set; }
        [Required]
        [MinLength(5)]
        public string ConfirmPassword { get; set;}
    }
}

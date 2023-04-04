using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class SetPassword
    {

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Confirm Password doesn't Match")]
        public string ConfirmPassword { get; set; }



    }
}

using System.ComponentModel.DataAnnotations;

namespace LoginPageTest.Models
{

    public class ResgisterUser
    {

        [Required(ErrorMessage = "Please Enter UserName..")]
        [Display(Name = "User Name")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Please Enter Password...")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Email..")]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}

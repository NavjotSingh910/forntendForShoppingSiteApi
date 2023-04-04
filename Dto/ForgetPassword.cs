using System.ComponentModel.DataAnnotations;

namespace LoginPageTest.Dto
{
    public class ForgetPassword
    {

        [Required(ErrorMessage = "Please Enter UserName..")]
        [Display(Name = "User Name")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Please Enter Email..")]
        [Display(Name = "Email")]
        public string Email { get; set; }


    }
}
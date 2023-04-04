using System.ComponentModel.DataAnnotations;

namespace LoginPageTest.Dto
{
    public class ResetPassword
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Token { get; set; }
    }
}


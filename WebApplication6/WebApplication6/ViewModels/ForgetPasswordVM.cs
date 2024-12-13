using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ForgetPasswordVM
    {
       [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

    }
}

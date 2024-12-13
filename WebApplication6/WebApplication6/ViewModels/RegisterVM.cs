
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class RegisterVM
	{
		public string FName { get; set; }
		public string LName { get; set; }
		[Required (ErrorMessage ="Email is required"), EmailAddress(ErrorMessage = "Email is required")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare("Password",ErrorMessage ="Confirm Password dosen`t match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		public bool Agree { get; set; }
	}
}

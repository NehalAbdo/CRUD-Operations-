using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class LoginVM
	{
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		public bool RememberMe { get; set; }

	}
}

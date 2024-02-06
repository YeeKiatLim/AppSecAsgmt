using System.ComponentModel.DataAnnotations;

namespace Assignment_1.ViewModels
{
	public class ChangePassword
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}$", ErrorMessage = "Password must be at least 12 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
		public string OldPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}$", ErrorMessage = "Password must be at least 12 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
		public string NewPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
	}
}

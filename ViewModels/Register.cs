using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace Assignment_1.ViewModels
{
	public class Register
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}$", ErrorMessage = "Passwords must be at least 12 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCard { get; set; }
		[Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string DeliveryAddress { get; set; }
    }
}

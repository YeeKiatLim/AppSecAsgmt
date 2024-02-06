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
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{12,}$", ErrorMessage = "Password must be at least 12 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.Text)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\s)[A-Za-z\s]{1,}$", ErrorMessage = "Name can only have letters and spaces")]
		public string FullName { get; set; }
        [Required]
        [DataType(DataType.CreditCard)]
		[RegularExpression(@"^(?=.*\d)[\d]{16,16}$", ErrorMessage = "Credit Card No must be 16 digits")]
		public string CreditCard { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^(?=.*\d)[\d]{8,8}$", ErrorMessage = "Mobile Phone No must be 8 digits")]
		public string PhoneNumber { get; set; }
		[Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }
        [Required]
        [DataType(DataType.Text)]
		[RegularExpression(@"^(?=.*-)(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\s)(?=.*\#)[-A-Za-z\d\s\#]{1,}$", ErrorMessage = "Delivery Address can only have letters, digits and spaces")]
		public string DeliveryAddress { get; set; }
        [Required]
		[DataType(DataType.Text)]
		public string AboutMe { get; set; }
    }
}

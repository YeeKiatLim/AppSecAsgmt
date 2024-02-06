using Assignment_1.Model;
using Assignment_1.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace Assignment_1.Pages
{
	public class RegisterModel : PageModel
	{
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }
		[BindProperty]
		public Register RModel { get; set; }

		public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public void OnGet()
		{
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                var user = new ApplicationUser()
				{
					UserName = HttpUtility.HtmlEncode(RModel.Email),
					Email = HttpUtility.HtmlEncode(RModel.Email),
					PhoneNumber = HttpUtility.HtmlEncode(RModel.PhoneNumber),
					FullName = HttpUtility.HtmlEncode(RModel.FullName),
					CreditCard = protector.Protect(RModel.CreditCard),
					Gender = HttpUtility.HtmlEncode(RModel.Gender),
					DeliveryAddress = HttpUtility.HtmlEncode(RModel.DeliveryAddress),
					AboutMe = HttpUtility.HtmlEncode(RModel.AboutMe)
				};

				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					return RedirectToPage("Login");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}
	}
}

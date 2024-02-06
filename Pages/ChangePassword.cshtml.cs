using Assignment_1.Model;
using Assignment_1.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace Assignment_1.Pages
{
    public class ChangePasswordModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }
		[BindProperty]
		public ChangePassword CModel { get; set; }

		public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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
				var user = await userManager.FindByEmailAsync(CModel.Email);
				if (user != null)
				{
					var check = await userManager.CheckPasswordAsync(user, CModel.OldPassword);
					if (check)
					{
						var result = await userManager.ChangePasswordAsync(user, CModel.OldPassword, CModel.NewPassword);
						if (result.Succeeded)
						{
							return RedirectToPage("Login");
						}

						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
					else
					{
						ModelState.AddModelError("", errorMessage: "Password does not match");
					}
				}
				else
				{
					ModelState.AddModelError("", errorMessage: "User does not exist");
				}
			}
			return Page();
		}
	}
}

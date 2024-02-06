using Assignment_1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

namespace Assignment_1.Pages
{
	[Authorize(Policy = "CanViewDetails", AuthenticationSchemes = "MyCookieAuth")]
	public class IndexModel : PageModel
	{
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor contxt;
		private UserManager<ApplicationUser> userManager;
		private SignInManager<ApplicationUser> signInManager;

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_logger = logger;
			contxt = httpContextAccessor;
			this.userManager = userManager;
			this.signInManager = signInManager;
        }

		[HttpGet]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnGet()
		{
            if (contxt.HttpContext.Session.GetString("LoggedIn") != null && contxt.HttpContext.Session.GetString("AuthToken") != null && Request.Cookies["AuthToken"] != null)
			{
				if (!contxt.HttpContext.Session.GetString("AuthToken").ToString().Equals(Request.Cookies["AuthToken"]))
				{
					return RedirectToPage("Login");
				}
				else
				{
                    var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                    var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                    var user = await userManager.FindByEmailAsync(contxt.HttpContext.Session.GetString("LoggedIn"));
					if (user != null)
					{
						ViewData["Username"] = user.UserName;
						ViewData["Email"] = user.Email;
						ViewData["PhoneNo"] = user.PhoneNumber;
						ViewData["PasswordHash"] = user.PasswordHash;
						ViewData["FullName"] = user.FullName;
						ViewData["Gender"] = user.Gender;
						ViewData["CreditCard"] = protector.Unprotect(user.CreditCard);
						ViewData["Address"] = user.DeliveryAddress;
						ViewData["About"] = user.AboutMe;
					}
                }
			}
			else{
                return RedirectToPage("Login");
            }
            return Page();
		}
	}
}
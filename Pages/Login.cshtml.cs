using Assignment_1.Model;
using Assignment_1.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.IO;
using System.Text.Json;

namespace Assignment_1.Pages
{
    public class LoginModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		[BindProperty]
		public Login LModel { get; set; }
		public class MyObject
		{
			public string success { get; set; }
			public List<String> ErrorMessage { get; set; }
		}
		
		public LoginModel(SignInManager<ApplicationUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public bool ValidateCaptcha()
		{
			bool result = true;
			string recaptchaResponse = Request.Form["g-recaptcha-response"];
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LeBfGApAAAAAJPdCro98ckS5gM6kTOKWdG5WMWS &response=" + recaptchaResponse);

			try
			{
				using (WebResponse wResponse = req.GetResponse())
				{ 
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();
						MyObject jsonObject = JsonSerializer.Deserialize<MyObject>(jsonResponse);
						result = Convert.ToBoolean(jsonObject.success);
					}
				}
				return result;
			}
			catch (WebException ex)
			{
				throw ex;
			}
		}

		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid && ValidateCaptcha())
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
				LModel.RememberMe, false);
				if (identityResult.Succeeded)
				{
					return RedirectToPage("Index");
				}
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}

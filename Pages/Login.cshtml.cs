using Assignment_1.Model;
using Assignment_1.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.IO;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using System.Security.Claims;
using System;

namespace Assignment_1.Pages
{
    public class LoginModel : PageModel
    {
		private readonly ILogger<LoginModel> _logger;
        private readonly IHttpContextAccessor contxt;
        private readonly SignInManager<ApplicationUser> signInManager;
		private readonly AuthDbContext authDbContext;
		[BindProperty]
		public Login LModel { get; set; }
		public class MyObject
		{
			public bool success { get; set; }
			public List<String> ErrorMessage { get; set; }
		}

		public LoginModel(ILogger<LoginModel> logger, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager, AuthDbContext authDbContext)
		{
			_logger = logger;
			contxt = httpContextAccessor;
			this.signInManager = signInManager;
			this.authDbContext = authDbContext;
		}

		public async Task<bool> ValidateCaptchaAsync()
		{
			bool result = true;
			string recaptchaResponse = Request.Form["g-recaptcha-response"];
			using (HttpClient client = new HttpClient())
			{
				var secret = "6LeBfGApAAAAAJPdCro98ckS5gM6kTOKWdG5WMWS";
				var uri = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={recaptchaResponse}";
				try
				{
					HttpResponseMessage response = await client.GetAsync(uri);

					if (response.IsSuccessStatusCode)
					{
						string jsonResponse = await response.Content.ReadAsStringAsync();
						MyObject jsonObject = JsonSerializer.Deserialize<MyObject>(jsonResponse);
						result = Convert.ToBoolean(jsonObject.success);
					}
					else
					{
						// Handle non-success status codes
						Console.WriteLine($"Error validating reCAPTCHA: {response.StatusCode}");
						result = false;
					}
					return result;
				}
				catch (WebException ex)
				{
					throw ex;
				}
			}
		}

		public void OnGet()
        {
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid && ValidateCaptchaAsync().Result)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
				LModel.RememberMe, false);
				if (identityResult.Succeeded)
				{
					//Create the security context
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, LModel.Email),
						new Claim(ClaimTypes.Email, LModel.Email),
						new Claim("Authorized", "Yes")
                    };
					var i = new ClaimsIdentity(claims, "MyCookieAuth");
					ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
					await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
					contxt.HttpContext.Session.SetString("LoggedIn", LModel.Email.ToString());
                    string guid = Guid.NewGuid().ToString();
					contxt.HttpContext.Session.SetString("AuthToken", guid);
					Response.Cookies.Append("AuthToken", guid);
                    var log = new AuditLog
                    {
                        UserId = LModel.Email,
                        Action = "Logged In",
                        Time = DateTime.Now,
                    };
                    authDbContext.AuditLogTable.Add(log);
                    authDbContext.SaveChanges();
                    return RedirectToPage("Index");
				}
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}

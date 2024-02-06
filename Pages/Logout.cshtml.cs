using Assignment_1.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment_1.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly ILogger<LogoutModel> _logger;
		private readonly IHttpContextAccessor contxt;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AuthDbContext authDbContext;

        public LogoutModel(ILogger<LogoutModel> logger, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthDbContext authDbContext)
		{
			_logger = logger;
			contxt = httpContextAccessor;
			this.userManager = userManager;
			this.signInManager = signInManager;
            this.authDbContext = authDbContext;
        }

		public void OnGet()
        {
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostLogoutAsync()
		{
            var user = await userManager.GetUserAsync(User);
            await signInManager.SignOutAsync();
            var log = new AuditLog
            {
                UserId = user.Email,
                Action = "Logged Out",
                Time = DateTime.Now,
            };
            authDbContext.AuditLogTable.Add(log);
            authDbContext.SaveChanges();
            contxt.HttpContext.Session.Clear();
			if (Request.Cookies[".AspNetCore.Session"] != null)
			{
				Response.Cookies.Delete(".AspNetCore.Session");
			}
			if (Request.Cookies["AuthToken"] != null)
			{
				Response.Cookies.Delete("AuthToken");
			}
			return RedirectToPage("Login");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}

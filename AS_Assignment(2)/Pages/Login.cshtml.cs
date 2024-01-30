using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Optimization;
using Microsoft.AspNetCore.Http;
using System.Web;

using Microsoft.AspNetCore.Identity;
using AS_Assignment_2_.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using AS_Assignment_2_.Model;


namespace AS_Assignment_2_.Pages
{
	public class LoginModel : PageModel
	{
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly SessionManager sessionManager;


        public ILogger<LoginModel> logger { get; private set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, SessionManager sessionManager)
		{
			this.signInManager = signInManager;
            this.logger = logger;
            this.sessionManager = sessionManager;
        }
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
            Console.WriteLine("OnPostAsync method executed.");
            logger.LogInformation("OnPostAsync method executed.");
            if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
				LModel.RememberMe, false);

                if (identityResult.Succeeded)
                {
                    // Log success or use debugging to check if this block is executed
                    Console.WriteLine("Login successful. Redirecting to Index.");
                    logger.LogInformation("Login successful. Redirecting to Index.");

                    //Create the security context
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, "c@c.com"),
                        new Claim(ClaimTypes.Email, "c@c.com"),

                        new Claim("Department", "HR")
                    };

                    var i = new ClaimsIdentity(claims, "MyCookieAuth");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                    HttpContext.Session.SetString("LoggedIn", LModel.Email);

                    string guid = Guid.NewGuid().ToString();

                    HttpContext.Session.SetString("AuthToken", guid);
                    HttpContext.Response.Cookies.Append("AuthToken", guid);

                    sessionManager.AddSession(LModel.Email, HttpContext.Session.Id);

                    if (sessionManager.IsUserLoggedInFromAnotherDevice(LModel.Email, HttpContext.Session.Id))
                    {
                        // Handle multiple logins (e.g., log out the user, show a warning)
                        ModelState.AddModelError("", "Multiple logins detected. Please log out from other devices.");
                        await signInManager.SignOutAsync();
                        return Page();
                    }



                    return RedirectToPage("Index");

                    
                }
                else
                {
                    // Log or debug to check why login failed
                    Console.WriteLine("Login failed. Username or password incorrect.");
                    ModelState.AddModelError("", "Username or Password incorrect");
                }
         
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}
           
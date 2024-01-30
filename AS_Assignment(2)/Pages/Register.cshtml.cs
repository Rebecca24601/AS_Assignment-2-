using AS_Assignment_2_.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using AS_Assignment_2_.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

namespace AS_Assignment_2_.Pages
{
	//Initialize the build-in ASP.NET Identity
	public class RegisterModel : PageModel
	{
        
        private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }

        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
		public Register RModel { get; set; }

        [BindProperty]
        public string? RecaptchaToken { get; set; }



        public void OnGet()
        {



        }
        public RegisterModel(UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
            this.roleManager = roleManager;

        }

		public async Task<IActionResult> OnPostAsync()
		{
            // Check if the email already exists
            var existingUser = await userManager.FindByEmailAsync(RModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("RModel.Email", "Email is already registered");
                return Page();
            }

            if (ModelState.IsValid)
			{
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");


                var user = new ApplicationUser()
				{
					UserName = RModel.Email,
					Email = RModel.Email,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Gender = RModel.Gender,
                    NRIC = protector.Protect(RModel.NRIC),
                    Password = RModel.Password,
                    ConfirmPassword = RModel.ConfirmPassword,
                    DateofBirth = RModel.DateofBirth,
                    Resume = RModel.Resume,
                    WhoAmI = RModel.WhoAmI,

                    
                };
                var protecteddata = protector.Protect(RModel.NRIC);

                var unprotectedData = protector.Unprotect(protecteddata);

                ViewData["unprotectedmessage"] = unprotectedData;

                //Create the Admin role if NOT exist
                IdentityRole role = await roleManager.FindByIdAsync("Admin");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role admin failed");
                    }
                }

                

                var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
                    // Use UserManager to add claims
                    await userManager.AddClaimAsync(user, new Claim("NRIC", protector.Protect(RModel.NRIC)));
                    //Add users to Admin Role
                    result = await userManager.AddToRoleAsync(user, "Admin");

                    await signInManager.SignInAsync(user, false);
					return RedirectToPage("Index");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

            // Get the reCAPTCHA token from the form
            RecaptchaToken = Request.Form["g-recaptcha-response"];

            if (string.IsNullOrEmpty(RecaptchaToken))
            {
                // Handle the case where reCAPTCHA token is not provided
                ModelState.AddModelError("", "reCAPTCHA validation failed. Please try again.");
                return Page();
            }

            // Your other code

            // Verify reCAPTCHA on the server side
            var secretKey = "6Leq9hIpAAAAAHrQ_vS6J4oE4yPwELct17eXBxVr";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify";

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(apiUrl, new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("secret", secretKey),
            new KeyValuePair<string, string>("response", RecaptchaToken)
        }));

                var responseBody = await response.Content.ReadAsStringAsync();
                var recaptchaResult = JsonConvert.DeserializeObject<RecaptchaResult>(responseBody);

                if (recaptchaResult.Success)
                {
                    // reCAPTCHA verification successful, proceed with form processing
                    // ...
                }
                else
                {
                    // reCAPTCHA verification failed, handle accordingly
                    ModelState.AddModelError("", "reCAPTCHA validation failed. Please ensure that you are not a robot.");
                    return Page();
                }
            }

            return Page();
		}

        

        
    }

    public class RecaptchaResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

	



}


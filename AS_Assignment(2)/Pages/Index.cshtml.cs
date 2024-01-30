using AS_Assignment_2_.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Security.Claims;
using System.Web;

namespace AS_Assignment_2_.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public IndexModel(ILogger<IndexModel> logger, SignInManager<ApplicationUser> signInManager, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            _signInManager = signInManager;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public void OnGet()
        {
            OnGetPage();
        }

        public IActionResult OnGetPage()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var protectedNric = User.FindFirstValue("NRIC");

                if (string.IsNullOrEmpty(protectedNric))
                {
                    // Handle the case where protectedNric is not found
                    ViewData["LoggedInMessage"] = "NRIC not found.";
                    ViewData["MessageColor"] = "red";
                    return Page();
                }

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                try
                {
                    var decryptedNric = protector.Unprotect(protectedNric);

                    ViewData["LoggedInMessage"] = "You are logged in!";
                    ViewData["DecryptedNRIC"] = decryptedNric;
                    ViewData["MessageColor"] = "green";
                }
                catch (Exception ex)
                {
                    // Log the details of the exception and the protected NRIC
                    _logger.LogError(ex, "Error during NRIC decryption. ProtectedNric: {ProtectedNric}", protectedNric);
                    ViewData["LoggedInMessage"] = $"Error during NRIC decryption: {ex.Message}";
                    ViewData["MessageColor"] = "red";
                }
            }
            else
            {
                return RedirectToPage("Login");
            }

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            _signInManager.SignOutAsync().Wait(); // Wait for the sign-out to complete
            return RedirectToPage("Login");
        }
    }




}

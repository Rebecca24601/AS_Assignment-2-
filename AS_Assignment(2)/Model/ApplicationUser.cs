using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AS_Assignment_2_.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

		public string Gender { get; set; }

		public string NRIC { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public string DateofBirth { get; set; }

		[DataType(DataType.Upload)]
		[FileExtensions(Extensions = ".docx,.pdf", ErrorMessage = "Please upload a valid .docx or .pdf file.")]
		public string Resume { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Special characters are allowed.")]
        public string WhoAmI { get; set; }
	}
}

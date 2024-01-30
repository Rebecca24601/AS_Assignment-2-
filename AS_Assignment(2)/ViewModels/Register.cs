using System.ComponentModel.DataAnnotations;

namespace AS_Assignment_2_.ViewModels
{
	public class Register
	{
		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		
		[DataType(DataType.Text)]
		public string Gender { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string NRIC { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }

		
		[DataType(DataType.Date)]
		public string DateofBirth { get; set; }


		[DataType(DataType.Upload)]
		[FileExtensions(Extensions = ".docx,.pdf", ErrorMessage = "Please upload a valid .docx or .pdf file.")]
		public string Resume { get; set; }


		[DataType(DataType.Text)]
		[RegularExpression(@"^[^\s]+$", ErrorMessage = "Special characters are allowed.")]
		public string WhoAmI { get; set; }
	}
}

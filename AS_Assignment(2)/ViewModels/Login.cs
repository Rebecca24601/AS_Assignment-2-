﻿using System.ComponentModel.DataAnnotations;

namespace AS_Assignment_2_.ViewModels
{
	public class Login
	{


		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }

		

	}
	
}

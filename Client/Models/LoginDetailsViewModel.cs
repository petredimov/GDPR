using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class LoginDetailsViewModel
    {
		[Display(Name = "Username/Email")]
		[Required]
		public string Username { get; set; }

		[DisplayName("Password")]
		[Required]
		public string Password { get; set; }

		[DisplayName("Email")]
		public string Email { get; set; }
    }
}
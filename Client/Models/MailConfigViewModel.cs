using DatabaseNamespace;
using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
	public class MailConfigViewModel
	{
		public int Id { get; set; }
		[DisplayName("Username")]
		[Required]
		public string Username { get; set; }

		[Required]
		[DisplayName("Password")]
		public string Password { get; set; }

		[Required]
		[DisplayName("Port")]
		public int Port { get; set; }


		[DisplayName("Enable ssl")]
		public bool EnableSsl { get; set; }

		[DisplayName("Server")]
		[Required]
		public string Host { get; set; }
		//[Required]

		[DisplayName("Mail type")]
		public MailServerType MailType { get; set; }
		//[Required]

		[DisplayName("From")]
		public string From { get; set; }
		public string UserId { get; set; }

		public MailConfiguration ToDbModel()
		{
			return new MailConfiguration
			{
				 Id = this.Id,
				 EnableSsl = this.EnableSsl,
				 From = this.From,
				 Host = this.Host,
				 MailType = this.MailType,
				 Password = this.Password,
				 Port = this.Port,
				 Username = this.Username,
				 UserId = this.UserId
			};
		}
	}
}
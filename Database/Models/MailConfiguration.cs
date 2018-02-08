using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace.Models
{
	public class MailConfiguration
	{
		public int Id { get; set; }

		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
		public string Host { get; set; }
		public MailServerType MailType { get; set; }
		public string From { get; set; }
	}
}

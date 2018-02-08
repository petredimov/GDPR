using Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Client.Models
{
	public class AuditViewModel
	{
		public int Id { get; set; }

		[DisplayName("Action")]
		public LogLevel Action { get; set; }

		[DisplayName("Date")]
		public DateTime Date { get; set; }

		public string UserId { get; set; }

		[DisplayName("Description")]
		public string Message { get; set; }
	}
}
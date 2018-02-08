using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Client.Models
{
	public class DashboardViewModel
	{ 
		[DisplayName("Number of users")]
		public int NumberOfUsers { get; set; }

		[DisplayName("Number of requests")]
		public int NumberOfRequests { get; set; }

		[DisplayName("Number of contracts")]
		public int NumberOfContrats { get; set; }
	}
}
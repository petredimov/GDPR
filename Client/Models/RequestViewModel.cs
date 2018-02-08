using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace Client.Models
{
	public class RequestViewModel
	{
		public int Id { get; set; }
		public DateTime DateRequested { get; set; }
		public DateTime ValidUntil { get; set; }
		public string Url { get; set; }
		public ContractViewModel Contract { get; set; }
        public string ContractId { get; set; }
	}
}
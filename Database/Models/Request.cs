using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace.Models
{
	public class Request
	{
		public int Id { get; set; }
		public DateTime DateRequested { get; set; }
		public DateTime ValidUntil { get; set; }
		public string Url { get; set; }

		[ForeignKey("ContractId")]
		public virtual Contract Contract { get; set; }
		public string ContractId { get; set; }
	}
}

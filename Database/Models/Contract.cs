using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
    public class Contract
    {
        public string Id { get; set; }
		public string UserID { get; set; }
		[ForeignKey("UserID")]
		public virtual User User { get; set; }
		public string ManagerID { get; set; }
		[ForeignKey("ManagerID")]
		public virtual User Manager { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime? SigningDate { get; set; }
        public ContractStatus Status { get; set; }
		public int ContractTypeId { get; set; }
		[ForeignKey("ContractTypeId")]
		public ContractType Type { get; set; }	
		public virtual ICollection<Request> Requests { get; set; }
	}
}

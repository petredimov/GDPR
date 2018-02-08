using Client.Mappers;
using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class ContractViewModel
    {
        public string Id { get; set; }
        public UserViewModel User { get; set; }
        public string UserId { get; set; }
        public UserViewModel Manager { get; set; }
        public string ManagerId { get; set; }

		[DisplayName("Date sent")]
		public DateTime InviteDate { get; set; }

		[DisplayName("Date signed")]
		public DateTime? SigningDate { get; set; }

		[DisplayName("Agree")]
		public bool Status { get; set; }

		[DisplayName("Type")]
		public ContractTypeViewModel Type { get; set; }

        public int TypeId { get; set; }

        public Contract ToDbModel()
        {
            return new Contract
            {
                Id = this.Id,
                InviteDate = this.InviteDate,
                Manager = this.Manager?.ToDbModel(),
                ManagerID=this.ManagerId,
                SigningDate = this.SigningDate,
                Status = (this.Status==false)?ContractStatus.NotSigned:ContractStatus.Signed,
                User = this.User?.ToDbModel(),
                UserID=this.UserId,
                Type = this.Type?.ToDbModel()
            };
        }
    }
}
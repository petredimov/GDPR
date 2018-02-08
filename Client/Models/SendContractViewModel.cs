using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class SendContractViewModel
    {
        public string UserId { get; set; }
        public int ManagerId { get; set; }
        public List<ContractTypeViewModel> ListContractTypes { get; set; }
    }
}
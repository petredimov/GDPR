using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
    public class ContractUser : User
    {
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public ContractManager Manager { get; set; }
    }
}

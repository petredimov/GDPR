using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
    public class ContractManager : User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ICollection<ContractUser> Users { get; set; }
        
    }
}

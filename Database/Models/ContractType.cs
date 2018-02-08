using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
    public class ContractType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual User Manager{ get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
    public class Audit
    {
        public int Id { get; set; }
        public int Action { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
		public string Message { get; set; }
	}
}

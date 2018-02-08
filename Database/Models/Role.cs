using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
	public class Role : IdentityRole<string, IdentityUserRole>
	{
		public string Description { get; set; }
	}
}

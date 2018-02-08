using DatabaseNamespace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
	public class User : IdentityUser // <string, IdentityUserLogin, Role, IdentityUserClaim>
	{

		[Required]
		public string Name { get; set; }

		public string Address { get; set; }

		public string CompanyName { get; set; }
			
		//public int RoleId { get; set; }
		//[ForeignKey("RoleId")]
		//public virtual Role Role { get; set; }

		public string UserId { get; set; }

		public ICollection<User> Users { get; set; }

		[InverseProperty("User")]
		public ICollection<Contract> UserContracts { get; set; }

		[InverseProperty("Manager")]
		public ICollection<Contract> ManagerContracts { get; set; }

        public ICollection<ContractType> ContractTypes { get; set; }
 
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}
}

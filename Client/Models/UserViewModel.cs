using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
	public class UserViewModel
	{
		public string Id { get; set; }

		[Required]
		[DisplayName("Name")]
		public string Name { get; set; }

		[Required]
		[EmailAddress(ErrorMessage = "Wrong format")]
		[DisplayName("Email")]
		public string Email { get; set; }

		[DisplayName("Address")]
		public string Address { get; set; }

		[DisplayName("Company")]
		public string CompanyName { get; set; }

		[Required]
		[DisplayName("Username")]
		public string Username { get; set; }

		[Required]
		[DisplayName("Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string RoleId { get; set; }
		public string UserId { get; set; }

		public virtual bool EmailConfirmed { get; set; }

		public virtual string SecurityStamp { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual bool PhoneNumberConfirmed { get; set; }

		public virtual bool TwoFactorEnabled { get; set; }

		public virtual DateTime? LockoutEndDateUtc { get; set; }

		public virtual bool LockoutEnabled { get; set; }

		public virtual int AccessFailedCount { get; set; }

		public virtual string ConcurrencyStamp { get; set; }

		public virtual string Rolename { get; set; }
		
		public ICollection<UserViewModel> Users { get; set; }
		public List<ContractViewModel> UserContracts { get; set; }

		public List<ContractViewModel> ManagerContracts { get; set; }


		public User ToDbModel()
		{
			return new User
			{
				Id = this.Id,
				Email = this.Email,
				Name = this.Name,
				PasswordHash = this.Password,
				UserName = this.Username,
				UserContracts = this.UserContracts?.Select(c => c.ToDbModel()).ToList(),
				ManagerContracts = this.ManagerContracts?.Select(c => c.ToDbModel()).ToList(),
				Address = this.Address,
				CompanyName = this.CompanyName,
				AccessFailedCount = this.AccessFailedCount,
				EmailConfirmed = this.EmailConfirmed,
				LockoutEnabled = this.LockoutEnabled,
				LockoutEndDateUtc = this.LockoutEndDateUtc,
				PhoneNumber = this.PhoneNumber,
				PhoneNumberConfirmed = this.PhoneNumberConfirmed,
				SecurityStamp = this.SecurityStamp,
				TwoFactorEnabled = this.TwoFactorEnabled,
				UserId = this.UserId,
			};
		}
	}
}
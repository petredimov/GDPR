using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseNamespace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DatabaseNamespace
{
	public class DatabaseContext : IdentityDbContext<User> //<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
	{

		public static string connectionString = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", DbCredentials.DbHost, DbCredentials.DbName, DbCredentials.DbUser, DbCredentials.DbPassword);// @"Data Source=gdprhosted.database.windows.net;Initial Catalog=gdpr;User Id=gdpr;Password=qwerty12#3;";

		public DatabaseContext() : base(connectionString)
		{
			this.Database.CommandTimeout = 120;
        }

        public static DatabaseContext Create()
		{
			return new DatabaseContext();
		}

		public virtual DbSet<Audit> Audits { get; set; }
		public virtual DbSet<Contract> Contracts { get; set; }
		public virtual DbSet<IdentityUserRole> UserRoles { get; set; }		
		public virtual DbSet<ContractType> ContractTypes { get; set; }

		public virtual DbSet<Request> Requests { get; set; }

		public virtual DbSet<IdentityUserClaim> UserClaims { get; set; }
		public virtual DbSet<IdentityUserLogin> UserLogins { get; set; }	
		
		public virtual DbSet<MailConfiguration> MailConfigurations { get; set; }
	}
}

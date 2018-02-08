namespace DatabaseNamespace.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DatabaseNamespace;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Microsoft.AspNet.Identity;
    using DatabaseNamespace.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseNamespace.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseNamespace.DatabaseContext context)
        {

			User user = null;

			//password is Qwerty12#3
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var UserManager = new UserManager<User>(new UserStore<User>(context));
			if (!context.Roles.Any())
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Admin";
				roleManager.Create(role);

				user = new User
				{
					UserName = "admin@cegeka.com",
					PhoneNumber = "08869879",
					Email = "admin@cegeka.com",
					Name = "Admin",
					SecurityStamp = DateTime.Now.ToString(),
					CompanyName = "Cegeka"
				};

				string userPWD = "Qwerty12#3";

				var chkUser = UserManager.Create(user, userPWD);

				//Add default User to Role Admin   
				if (chkUser.Succeeded)
				{
					var result1 = UserManager.AddToRole(user.Id, "Admin");
				}

				role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Manager";
				roleManager.Create(role);

				role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "User";
				roleManager.Create(role);				
			}
            if (!context.MailConfigurations.Any())
            {
                MailConfiguration mailConfig = new MailConfiguration();
                mailConfig.Username = "gdprcontract";
                mailConfig.Password = "qwerty12#3";
                mailConfig.Host = "smtp.gmail.com";
                mailConfig.Port = 587;
                mailConfig.MailType = MailServerType.SMTP;
                mailConfig.From = "MailServerType";
                mailConfig.EnableSsl = true;
                context.MailConfigurations.Add(mailConfig);
            }

                //User manager = context.Users.Add(new User()
                //{
                //	Name = "Trajce Drvarceto",
                //	Email = "trajce.drvarceto@gmail.com",
                //	RoleId = managerRole.Id,
                //	UserName = "trajced",
                //	Password = "trajced",
                //	Users = new List<User>()
                //		{
                //			new User() { Email = "petre.dimov@gmail.com", Name = "Petre", Address = "Skopje 1000", RoleId = userRole.Id, UserName = "petre", Password="petre" },

                //			new User() { Email = "sanja.vitanova@gmail.com", Name = "Sanja", Address = "Skopje 1000", RoleId = userRole.Id, UserName = "sanja", Password = "sanja" }
                //		}
                //}
                //);
            //    byte[] data = new byte[100];// File.ReadAllBytes(@"C:\agreement_example.pdf");

            //context.ContractTypes.Add(new ContractType { Name = "Personal contract", Data = data });
            //context.ContractTypes.Add(new ContractType { Name = "1 year contract", Data = data });
            //context.ContractTypes.Add(new ContractType { Name = "Full year contract", Data = data });
            //context.ContractTypes.Add(new ContractType { Name = "Data contract", Data = data });

            context.SaveChanges();
        }
    }
}

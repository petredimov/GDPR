using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseNamespace;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DbServices.UserServices
{
    public class UserService : IUserService
    {
		/// <summary>
		/// Remove user with user id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>boolean</returns>
        public bool Delete(string userId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                User user = context.Users.FirstOrDefault(c => c.Id == userId);
                context.Users.Remove(user);
                return context.SaveChanges() > 0;
            }
        }

		/// <summary>
		/// Get user by specific id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        public User Get(string id)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Users.Include(c=>c.Users).Include(c=>c.UserContracts).Include(c=>c.ManagerContracts).FirstOrDefault(c => c.Id == id);
            }
        }

		/// <summary>
		/// Get all users from database
		/// </summary>
		/// <returns></returns>
        public List<User> GetAll()
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Users.Include(c=>c.Roles).ToList();
            }
        }

		/// <summary>
		/// Get all child users for specific user id 
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>boolean</returns>
		public List<User> GetByUserID(string userId)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.Users.Include(c => c.Roles).Where(c => c.UserId == userId).ToList();
			}
		}

		/// <summary>
		/// Insert User to database
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public bool Insert(User user)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                return context.SaveChanges() > 0;
            }
        }

		/// <summary>
		/// Update specific user in database
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
        public bool Update(User user)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                context.Users.AddOrUpdate(user);
                return context.SaveChanges() > 0;
            }
        }

		/// <summary>
		/// Get user role by specific name
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public string GetRoleIdByName(string roleName)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
				return roleManager.FindByName(roleName).Id;
			}
				
		}		
	}
}

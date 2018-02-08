using DatabaseNamespace;
using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
	public class MailConfigurationService : IMailConfigurationService
	{
		/// <summary>
		/// Add mail configuration to database
		/// </summary>
		/// <param name="configuration"></param>
		public void Add(MailConfiguration configuration)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				context.MailConfigurations.Add(configuration);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Get all mail configurations
		/// </summary>
		/// <returns></returns>
		public List<MailConfiguration> GetAll()
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.MailConfigurations.ToList();
			}
		}

		/// <summary>
		/// Get mail configuration by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public MailConfiguration GetById(int id)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.MailConfigurations.FirstOrDefault(c => c.Id == id);
			}
		}

		/// <summary>
		/// Get default mail configuration in database
		/// </summary>
		/// <returns></returns>
		public MailConfiguration GetMailConfiguration()
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.MailConfigurations.FirstOrDefault();
			}
		}

		/// <summary>
		/// Remove mail configuration
		/// </summary>
		/// <param name="mailConfigurationId"></param>
        public void Remove(int mailConfigurationId)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				MailConfiguration config = context.MailConfigurations.FirstOrDefault(c => c.Id == mailConfigurationId);
				if (config != null)
				{
					context.MailConfigurations.Remove(config);
					context.SaveChanges();
				}
			}
		}

		/// <summary>
		/// Update specific mail configuration 
		/// </summary>
		/// <param name="configuration"></param>
		public void Update(MailConfiguration configuration)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				context.MailConfigurations.AddOrUpdate(configuration);
				context.SaveChanges();
			}
		}
	}
}

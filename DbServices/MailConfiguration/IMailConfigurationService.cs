using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
	public interface IMailConfigurationService
	{
		MailConfiguration GetMailConfiguration();
		void Add(MailConfiguration configuration);
		void Update(MailConfiguration configuration);
		void Remove(int configurationId);
		List<MailConfiguration> GetAll();
		MailConfiguration GetById(int id);
	}
}

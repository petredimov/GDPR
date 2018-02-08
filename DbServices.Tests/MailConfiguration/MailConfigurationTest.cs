using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DatabaseNamespace;
using DatabaseNamespace.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DbServices.Tests
{
	[TestClass]
	public class MailConfigurationTest
	{
		Mock<DbSet<MailConfiguration>> mockSet = new Mock<DbSet<MailConfiguration>>();

		List<MailConfiguration> data = new List<MailConfiguration>();

		[TestInitialize]
		public void InitializeTest()
		{
			data = new List<MailConfiguration>()
			{
				new MailConfiguration { Id = 1, From = "Mail1@mail.mk", EnableSsl = true, Host = "smtp.mail.mk", MailType = MailServerType.SMTP, Port = 500, Username = "username1"},
				new MailConfiguration { Id = 2, From = "Mail2@mail.mk", EnableSsl = true, Host = "smtp.mail.mk", MailType = MailServerType.SMTP, Port = 500, Username = "username2"},
				new MailConfiguration { Id = 3, From = "Mail2@mail.mk", EnableSsl = true, Host = "smtp.mail.mk", MailType = MailServerType.SMTP, Port = 500, Username = "username3"},
			};

			mockSet.As<IQueryable<MailConfiguration>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
			mockSet.As<IQueryable<MailConfiguration>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
			mockSet.As<IQueryable<MailConfiguration>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
			mockSet.As<IQueryable<MailConfiguration>>().Setup(m => m.GetEnumerator()).Returns(() => data.AsQueryable().GetEnumerator());

			var mockContext = new Mock<DatabaseContext>();

			mockContext.Setup(c => c.MailConfigurations).Returns(mockSet.Object);
			ContextFactory.SetContext(mockContext.Object);
		}

		[TestMethod]
		public void GetAllMailConfigurationsTest()
		{
			IMailConfigurationService mailConfigurationService = new MailConfigurationService();
			Assert.AreEqual(3, mailConfigurationService.GetAll().Count);
		}

		[TestMethod]
		public void GetMailConfigurationById()
		{
			IMailConfigurationService mailConfigurationService = new MailConfigurationService();
			MailConfiguration config = mailConfigurationService.GetById(1);
			Assert.AreEqual("username1", config.Username);
		}

		[TestMethod]
		public void InsertMailConfigurationTest()
		{
			mockSet.Setup(m => m.Add(It.IsAny<MailConfiguration>())).Callback<MailConfiguration>((entity) => data.Add(entity));

			IMailConfigurationService mailConfigurationService = new MailConfigurationService();
			mailConfigurationService.Add(new MailConfiguration() { Id = 4, From = "TestInsert"  });

			Assert.AreEqual(4, mailConfigurationService.GetAll().Count);
		}

		[TestMethod]
		public void DeleteMailConfigurationTest()
		{
			mockSet.Setup(m => m.Remove(It.IsAny<MailConfiguration>())).Callback<MailConfiguration>((entity) => data.Remove(entity));

			IMailConfigurationService mailConfigurationService = new MailConfigurationService();
			mailConfigurationService.Remove(3);
			Assert.AreEqual(2, mailConfigurationService.GetAll().Count);
		}
	}
}

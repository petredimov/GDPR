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
	public class AuditTest
	{
		Mock<DbSet<Audit>> mockSet = new Mock<DbSet<Audit>>();

		List<Audit> data = new List<Audit>();

		[TestInitialize]
		public void InitializeTest()
		{
			try
			{
				data = new List<Audit>() {
				new Audit { Id = 1, Action = 1, Date = DateTime.Now, Message = "test1", UserId = "1"},
				new Audit { Id = 2, Action = 1, Date = DateTime.Now, Message = "test1", UserId = "1"},
				new Audit { Id = 3, Action = 1, Date = DateTime.Now, Message = "test1", UserId = "2"},
			};


				mockSet.As<IQueryable<Audit>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
				mockSet.As<IQueryable<Audit>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
				mockSet.As<IQueryable<Audit>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
				mockSet.As<IQueryable<Audit>>().Setup(m => m.GetEnumerator()).Returns(() => data.AsQueryable().GetEnumerator());

				var mockContext = new Mock<DatabaseContext>();

				mockContext.Setup(c => c.Audits).Returns(mockSet.Object);
				ContextFactory.SetContext(mockContext.Object);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[TestMethod]
		public void GetAllAuditsTest()
		{
			IAuditService auditService = new AuditService();
			Assert.AreEqual(3, auditService.GetAll().Count);
		}

		[TestMethod]
		public void GetAuditsByUserTest()
		{
			IAuditService auditService = new AuditService();
			Assert.AreEqual(2, auditService.GetByUser("1").Count);
		}

		[TestMethod]
		public void InsertValueTest()
		{
			mockSet.Setup(m => m.Add(It.IsAny<Audit>())).Callback<Audit>((entity) => data.Add(entity));

			IAuditService auditService = new AuditService();
			auditService.Insert(new Audit() { Action = 1, Date = DateTime.Now, Message = "Test message", UserId = "00-00" });
			Assert.AreEqual(4, auditService.GetAll().Count);
		}
	}
}

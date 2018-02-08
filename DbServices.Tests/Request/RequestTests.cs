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
	public class RequestTests
	{
		Mock<DbSet<Request>> mockSet = new Mock<DbSet<Request>>();

		List<Request> data = new List<Request>();

		[TestInitialize]
		public void InitializeTest()
		{
			data = new List<Request>()
			{
				new Request { Id = 1, DateRequested = DateTime.Now, Url = "", ContractId = "2", ValidUntil = DateTime.Now},
				new Request { Id = 2, DateRequested = DateTime.Now, Url = "", ContractId = "2", ValidUntil = DateTime.Now},
				new Request { Id = 3, DateRequested = DateTime.Now, Url = "", ContractId = "2", ValidUntil = DateTime.Now},
			};

			mockSet.As<IQueryable<Request>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
			mockSet.As<IQueryable<Request>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
			mockSet.As<IQueryable<Request>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
			mockSet.As<IQueryable<Request>>().Setup(m => m.GetEnumerator()).Returns(() => data.AsQueryable().GetEnumerator());

			var mockContext = new Mock<DatabaseContext>();

			mockContext.Setup(c => c.Requests).Returns(mockSet.Object);
			ContextFactory.SetContext(mockContext.Object);
		}

		[TestMethod]
		public void GetAllRequestsTest()
		{
			IRequestService requestService = new RequestService();
			Assert.AreEqual(3, requestService.GetAllRequests().Count);
		}

		[TestMethod]
		public void GetRequestsByUserIdTests()
		{
			IRequestService requestService = new RequestService();
			Assert.AreEqual(2, requestService.GetRequestsByContractId("1").Count);
		}

		[TestMethod]
		public void InsertRequestTest()
		{
			mockSet.Setup(m => m.Add(It.IsAny<Request>())).Callback<Request>((entity) => data.Add(entity));

			IRequestService requestService = new RequestService();
			requestService.InsertRequest(new Request() { Id = 4, DateRequested = DateTime.Now, Url = "", ContractId = "2", ValidUntil = DateTime.Now });

			Assert.AreEqual(4, requestService.GetAllRequests().Count);
		}

		[TestMethod]
		public void DeleteRequestTest()
		{
			mockSet.Setup(m => m.Remove(It.IsAny<Request>())).Callback<Request>((entity) => data.Remove(entity));

			IRequestService requestService = new RequestService();
			requestService.RemoveRequest(3);
			Assert.AreEqual(2, requestService.GetAllRequests().Count);
		}
	}
}

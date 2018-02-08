using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DatabaseNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DbServices.Tests
{
	[TestClass]
	public class ContractTest
	{
		Mock<DbSet<Contract>> mockSet = new Mock<DbSet<Contract>>();

		List<Contract> data = new List<Contract>();

		[TestInitialize]
		public void InitializeTest()
		{
			data = new List<Contract>() {
				new Contract { Id = "1", Status = ContractStatus.Signed, InviteDate = DateTime.Now, SigningDate = DateTime.Now, ContractTypeId = 1, ManagerID = "1", UserID = "2"},
				new Contract { Id = "2", Status = ContractStatus.Signed, InviteDate = DateTime.Now, SigningDate = DateTime.Now, ContractTypeId = 1, ManagerID = "1", UserID = "2"},
				new Contract { Id = "3", Status = ContractStatus.NotSigned, InviteDate = DateTime.Now, SigningDate = DateTime.Now, ContractTypeId = 1, ManagerID = "2", UserID = "2"}
			};


			mockSet.As<IQueryable<Contract>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
			mockSet.As<IQueryable<Contract>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
			mockSet.As<IQueryable<Contract>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
			mockSet.As<IQueryable<Contract>>().Setup(m => m.GetEnumerator()).Returns(() => data.AsQueryable().GetEnumerator());

			var mockContext = new Mock<DatabaseContext>();

			mockContext.Setup(c => c.Contracts).Returns(mockSet.Object);
			ContextFactory.SetContext(mockContext.Object);
		}

		[TestMethod]
		public void GetContractValueByIdTest()
		{
			IContractService contractService = new ContractService();
			Assert.AreEqual(3, contractService.GetAll().Count);
		}

		[TestMethod]
		public void InsertContractTest()
		{
			mockSet.Setup(m => m.Add(It.IsAny<Contract>())).Callback<Contract>((entity) => data.Add(entity));

			IContractService contractService = new ContractService();
			contractService.Insert(new Contract() { InviteDate = DateTime.Now, SigningDate = DateTime.Now, ManagerID = "15", UserID = "16", Status = ContractStatus.Signed, ContractTypeId = 3 } );
			Assert.AreEqual(4, contractService.GetAll().Count);
		}

		[TestMethod]
		public void DeleteContractTest()
		{
			mockSet.Setup(m => m.Remove(It.IsAny<Contract>())).Callback<Contract>((entity) => data.Remove(entity));

			IContractService contractService = new ContractService();
			contractService.Delete("3");
			Assert.AreEqual(2, contractService.GetAll().Count);
			
		}

		[TestMethod]
		public void GetContractByIdTest()
		{
			IContractService contractService = new ContractService();
			Contract contract = contractService.GetContractById("1");
			Assert.AreEqual(ContractStatus.Signed, contract.Status);
		}
		
	}
}

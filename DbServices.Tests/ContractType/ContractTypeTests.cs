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
	public class ContractTypeTests
	{
		Mock<DbSet<ContractType>> mockSet = new Mock<DbSet<ContractType>>();

		List<ContractType> data = new List<ContractType>();

		[TestInitialize]
		public void InitializeTest()
		{
			data = new List<ContractType>()
			{
				new ContractType { ID = 1, ManagerId = "2", Name = "ContractType1" },
				new ContractType { ID = 2, ManagerId = "2", Name = "ContractType2" },
				new ContractType { ID = 3, ManagerId = "2", Name = "ContractType3" },
			};

			mockSet.As<IQueryable<ContractType>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
			mockSet.As<IQueryable<ContractType>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
			mockSet.As<IQueryable<ContractType>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
			mockSet.As<IQueryable<ContractType>>().Setup(m => m.GetEnumerator()).Returns(() => data.AsQueryable().GetEnumerator());

			var mockContext = new Mock<DatabaseContext>();

			mockContext.Setup(c => c.ContractTypes).Returns(mockSet.Object);
			ContextFactory.SetContext(mockContext.Object);
		}

		[TestMethod]
		public void GetAllContractTypesTest()
		{
			IContractTypeService contractTypeService = new ContractTypeService();
			Assert.AreEqual(3, contractTypeService.GetAll().Count);
		}

		[TestMethod]
		public void GetContractTypeById()
		{
			IContractTypeService contractTypeService = new ContractTypeService();
			ContractType contractType = contractTypeService.GetById(1);
			Assert.AreEqual("ContractType1", contractType.Name);
		}

		[TestMethod]
		public void InsertContractTypeTest()
		{
			mockSet.Setup(m => m.Add(It.IsAny<ContractType>())).Callback<ContractType>((entity) => data.Add(entity));

			IContractTypeService contractTypeService = new ContractTypeService();
			contractTypeService.Insert(new ContractType() { ID = 4, ManagerId = "2", Name = "ContractType4" });

			Assert.AreEqual(4, contractTypeService.GetAll().Count);
		}

		[TestMethod]
		public void DeleteContractTypeTest()
		{
			mockSet.Setup(m => m.Remove(It.IsAny<ContractType>())).Callback<ContractType>((entity) => data.Remove(entity));

			IContractTypeService contractTypeService = new ContractTypeService();
			contractTypeService.Delete(3);
			Assert.AreEqual(2, contractTypeService.GetAll().Count);
		}
	}
}

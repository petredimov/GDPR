using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseNamespace;
using System.Data.Entity.Migrations;

namespace DbServices
{
    public class ContractTypeService : IContractTypeService
    {
		/// <summary>
		/// Get ContractTypes by user id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>List of contract types</returns>
		public List<ContractType> GetContractTypesByUser(string userId)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.ContractTypes.Where(c => c.ManagerId == userId).ToList();
			}
		}

		/// <summary>
		/// Delete contract from database 
		/// </summary>
		/// <param name="contractTypeId"></param>
		/// <returns>boolean</returns>
		public bool Delete(int contractTypeId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                ContractType contractType = context.ContractTypes.FirstOrDefault(c => c.ID == contractTypeId);
                if (contractType != null)
                {
                    context.ContractTypes.Remove(contractType);
                    return context.SaveChanges() > 0;
                }
            }
            return false;
        }

		/// <summary>
		/// Get all contract types
		/// </summary>
		/// <returns></returns>
        public List<ContractType> GetAll()
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.ContractTypes.ToList();
            }
        }

		/// <summary>
		/// Get ContractType by contract type id
		/// </summary>
		/// <param name="contractTypeId"></param>
		/// <returns>ContractType object</returns>
        public ContractType GetById(int contractTypeId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.ContractTypes.FirstOrDefault(c => c.ID == contractTypeId);
            }
        }

		/// <summary>
		/// Insert ContractType in database
		/// </summary>
		/// <param name="contractType"></param>
		/// <returns>boolean</returns>
        public bool Insert(ContractType contractType)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                context.ContractTypes.Add(contractType);
                return context.SaveChanges() > 0;
            }
        }

		/// <summary>
		/// Update specific contract type
		/// </summary>
		/// <param name="contractType"></param>
		/// <returns></returns>
        public bool Update(ContractType contractType)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                context.ContractTypes.AddOrUpdate(contractType);
                return context.SaveChanges() > 0;
            }
        }
    }
}

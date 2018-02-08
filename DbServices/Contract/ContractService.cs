using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseNamespace;
using System.Data.Entity.Migrations;
using System.Data.Entity;

namespace DbServices
{
    public class ContractService : IContractService
    {
		/// <summary>
		/// Delete contract from database
		/// </summary>
		/// <param name="contractId"></param>
		/// <returns>boolean</returns>
        public bool Delete(string contractId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                Contract contract = context.Contracts.FirstOrDefault(c => c.Id == contractId);
                if (contract != null)
                {
                    context.Contracts.Remove(contract);
                    return context.SaveChanges() > 0;
                }
            }
            return false;
        }

		/// <summary>
		/// Get all contracts from database
		/// </summary>
		/// <returns>List of contracts</returns>
        public List<Contract> GetAll()
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Contracts.ToList();
            }
        }

		/// <summary>
		/// Get contracts by user
		/// </summary>
		/// <param name="userID"></param>
		/// <returns>List of contracts</returns>
        public List<Contract> GetContractsByManagerID(string managerId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Contracts.Include(c => c.Requests).Where(c => c.ManagerID == managerId).ToList();
            }
        }

		/// <summary>
		/// Get contracts by manager id
		/// </summary>
		/// <param name="managerId"></param>
		/// <returns>List of contracts</returns>
		public List<Contract> GetContractsByUserID(string userId)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.Contracts.Include(c => c.Requests).Where(c => c.UserID == userId).ToList();
			}
		}

		/// <summary>
		/// Insert contract in database
		/// </summary>
		/// <param name="contract"></param>
		/// <returns></returns>
		public Contract Insert(Contract contract)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                Contract newContract=context.Contracts.Add(contract);
                if(context.SaveChanges() > 0)
                {
                    return newContract;
                }
                return null;
            }
        }

		/// <summary>
		/// Update contract in database
		/// </summary>
		/// <param name="contract"></param>
		/// <returns>boolean</returns>
        public bool Update(Contract contract)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                context.Contracts.AddOrUpdate(contract);
                return context.SaveChanges() > 0;
            }
        }

		/// <summary>
		/// Get contracts for specific users send by manager id
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="managerID"></param>
		/// <returns>List sof contracts</returns>
        public List<Contract> GetContractsByUserAndManager(string userID, string managerID)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Contracts.Where(c => c.UserID == userID && c.ManagerID==managerID).ToList();
            }
        }

		/// <summary>
		/// Get contract by contract id
		/// </summary>
		/// <param name="contractId"></param>
		/// <returns>Contract object</returns>
        public Contract GetContractById(string contractId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Contracts.FirstOrDefault(c => c.Id == contractId);
            }
        }
    }
}

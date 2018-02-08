using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseNamespace;
using System.Data.Entity.Migrations;
using System.Data.Entity;

namespace DbServices.User
{
    public class ContractUserService : IContractUserService
    {
        public bool Delete(int Id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ContractUser contractUser = context.ContractUsers.FirstOrDefault(c => c.Id == Id);
                context.ContractUsers.Remove(contractUser);
                return context.SaveChanges() > 0;
            }
        }

		public ContractUser Get(int Id)
		{
			using (DatabaseContext context = new DatabaseContext())
			{
				ContractUser contractUser = context.ContractUsers.Include(c=>c.Manager).FirstOrDefault(c => c.Id == Id);				
				return contractUser;
			}
		}

		public List<ContractUser> GetAll()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.ContractUsers.Include(c => c.Manager).ToList();
            }
        }

		public List<ContractUser> GetByContractManagerId(int managerId)
		{
			using (DatabaseContext context = new DatabaseContext())
			{
				return context.ContractUsers.Include(c=>c.Manager).Where(c=>c.Manager.Id == managerId).ToList();
			}
		}

		public bool Insert(ContractUser contract)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.ContractUsers.Add(contract);
                return context.SaveChanges() > 0;
            }
        }

        public bool Update(ContractUser contract)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.ContractUsers.AddOrUpdate(contract);
                return context.SaveChanges() > 0;
            }
        }
    }
}

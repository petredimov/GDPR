using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.User
{
    public interface IContractUserService
    {
		ContractUser Get(int Id);
		List<ContractUser> GetAll();
		List<ContractUser> GetByContractManagerId(int managerId);

		bool Insert(ContractUser contract);
        bool Update(ContractUser contract);
        bool Delete(int Id);
    }
}

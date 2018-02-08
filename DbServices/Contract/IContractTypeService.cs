using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
    public interface IContractTypeService
    {
        bool Insert(ContractType contractType);
        bool Update(ContractType contractType);
        bool Delete(int contractTypeId);
        List<ContractType> GetAll();
		List<ContractType> GetContractTypesByUser(string userId);
		ContractType GetById(int contractTypeId);
    }
}

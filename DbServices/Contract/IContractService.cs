using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
    public interface IContractService
    {
        Contract Insert(Contract contract);
        bool Update(Contract contract);
        bool Delete(string contractId);
        List<Contract> GetAll();
        List<Contract> GetContractsByUserID(string userID);
        List<Contract> GetContractsByUserAndManager(string userID,string managerID);
        List<Contract> GetContractsByManagerID(string managerId);
        Contract GetContractById(string contractId);

    }
}

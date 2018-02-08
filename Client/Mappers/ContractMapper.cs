using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Client.Models;
using DatabaseNamespace;
using DbServices.UserServices;
using DbServices;

namespace Client.Mappers
{
    public class ContractMapper
    {
        public static ContractViewModel ToViewModel(Contract contract)
        {
			if (contract != null)
			{
                IUserService userService = new UserService();
                IContractTypeService contractTypeService = new ContractTypeService();
                return new ContractViewModel
				{
					Id = contract.Id,
					InviteDate = contract.InviteDate,
                    SigningDate=contract.SigningDate,
					Manager = UserMapper.ToViewModel(userService.Get(contract.ManagerID)),
                    ManagerId=contract.ManagerID,
					Status = contract.Status==0?false:true,
					User = UserMapper.ToViewModel(userService.Get(contract.UserID)),
                    UserId= contract.UserID,
                    Type = ContractTypeMapper.ToViewModel(contractTypeService.GetById(contract.ContractTypeId),false),
                    TypeId=contract.ContractTypeId
				};
			}
			return null;
        }
    }
}
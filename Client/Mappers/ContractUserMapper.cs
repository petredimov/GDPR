using Client.Models;
using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Mappers
{
    public class ContractUserMapper
    {
        public static ContractUserViewModel ToViewModel (ContractUser contractUser)
        {
			if (contractUser != null)
			{
				return new ContractUserViewModel
				{
					Id = contractUser.Id,
					Address = contractUser.Address,
					Name = contractUser.Name,
					CompanyName = contractUser.CompanyName,
					Email = contractUser.Email,
					//Contracts = contractUser.Contracts?.Select(c => ContractMapper.ToViewModel(c)).ToList(),
					//Manager = ContractManagerMapper.ToViewModel(contractUser.Manager)
				};
			}
			return null;
        }
    }
}
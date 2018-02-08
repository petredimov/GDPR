using Client.Models;
using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Mappers
{
    public class UserMapper
    {
        public static UserViewModel ToViewModel(User user)
        {
			if (user != null)
			{
				return new UserViewModel
				{
					Id = user.Id,
					Email = user.Email,
					Name = user.Name,
					Password = user.PasswordHash,
					Username = user.UserName,
					Address = user.Address,
					CompanyName = user.CompanyName,					
					PhoneNumber = user.PhoneNumber,		
					SecurityStamp = user.SecurityStamp,
					UserId = user.UserId,
					//Contracts = contractManager.Contracts?.Select(c => ContractMapper.ToViewModel(c)).ToList(),
					Users = user.Users?.Select(c => UserMapper.ToViewModel(c)).ToList(),
				};
			}
			return null;
        }
    }
}
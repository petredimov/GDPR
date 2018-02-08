using Client.Models;
using DatabaseNamespace.Models;
using DbServices;
using DbServices.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Mappers
{
	public class RequestMapper
	{
		public static RequestViewModel ToViewModel(Request request)
		{
            return new RequestViewModel
            {
                Id = request.Id,
                DateRequested = request.DateRequested,
                Url = request.Url,
                ValidUntil = request.ValidUntil,
                ContractId=request.ContractId,
                Contract = ContractMapper.ToViewModel(new ContractService().GetContractById(request.ContractId))
			};
		}
	}
}
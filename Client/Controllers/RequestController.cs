using Client.Mappers;
using Client.Models;
using DatabaseNamespace;
using DbServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{

    public class RequestController : Controller
    {
		// GET: Request
		[Authorize(Roles = "Manager, Admin")]
		public ActionResult Index()
        {
			List<RequestViewModel> requests = new List<RequestViewModel>();

			IRequestService requestService = new RequestService();
			string userId = User.Identity.GetUserId();

			if (User.IsInRole(Roles.Admin.ToString()))
			{
				if (!String.IsNullOrEmpty(userId))
				{
					requests = requestService.GetAllRequests().Select(c => RequestMapper.ToViewModel(c)).ToList();
				}
			}
			else
			{
				IContractService contractService = new ContractService();
				List<Contract> contracts = contractService.GetContractsByManagerID(userId);

				foreach (Contract contract in contracts)
				{
					requests.AddRange(contract.Requests.Select(c => RequestMapper.ToViewModel(c)).ToList());
				}
			}

			return View(requests);
        }
    }
}
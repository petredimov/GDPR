using Client.Models;
using DatabaseNamespace;
using DbServices;
using DbServices.UserServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            DashboardViewModel model = null;

            IUserService userService = new UserService();

            if (User.IsInRole(Roles.Admin.ToString()))
            {
                model = new DashboardViewModel
                {
                    NumberOfRequests = new RequestService().GetAllRequests().Count,
                    NumberOfUsers = new UserService().GetAll().Count,
                    NumberOfContrats = new ContractService().GetAll().Count
                };
            }
            else
            {
                User user = userService.Get(User.Identity.GetUserId());
                IContractService contractService = new ContractService();

                IRequestService requestService = new RequestService();
                List<Contract> contracts = new List<Contract>();
                if (User.IsInRole(Roles.Manager.ToString()))
                {
                    contracts = contractService.GetContractsByManagerID(user.Id);
                }
                else
                {
                    contracts = contractService.GetContractsByUserID(user.Id);
                }


                model = new DashboardViewModel
                {
                    // TODO:
                    NumberOfRequests = contracts.Sum(c => c.Requests.Count),
                    NumberOfUsers = user.Users.Count,
                    NumberOfContrats = contracts.Count
                };

            }

            return View(model);
        }
    }
}
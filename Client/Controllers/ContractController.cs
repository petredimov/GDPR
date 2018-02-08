using Client.Mappers;
using Client.Models;
using DatabaseNamespace;
using DatabaseNamespace.Models;
using DbServices;
using DbServices.UserServices;
using Microsoft.AspNet.Identity;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class ContractController : Controller
    {
        public ActionResult Index()
        {
        
            return View();
        }
        public ActionResult GetAllContracts()
        {
            string userID=User.Identity.GetUserId();
            IContractService contractService = new ContractService();
            List<ContractViewModel> contracts= contractService.GetContractsByUserID(userID).Select(c=>ContractMapper.ToViewModel(c)).ToList();

            return View("Contracts", contracts);
        }
        public ActionResult Details(string contractId)
        {

            IContractService contractService = new ContractService();
            IContractTypeService contractTypeService = new ContractTypeService();
            ContractViewModel model=ContractMapper.ToViewModel(contractService.GetContractById(contractId));
            var contractTypeId = model.TypeId;
            model.Type = ContractTypeMapper.ToViewModel(contractTypeService.GetById(contractTypeId));
            return View("Index",model);
        }
        [HttpPost]
        public ActionResult SubmitContract(ContractViewModel model)
        {
            IContractService contractService = new ContractService();
            Contract contract=contractService.GetContractById(model.Id);
            if(model.Status)
            {
                contract.Status = ContractStatus.Signed;
                contract.SigningDate = DateTime.Now;
            }
            if (contractService.Update(contract))
            {
                IContractTypeService contractTypeService = new ContractTypeService();
                ContractType contractType=contractTypeService.GetById(contract.ContractTypeId);
                SendMailToUserAndManager(contract.UserID, contract.ManagerID, contractType.Name);                  
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        private void SendMailToUserAndManager(string userID, string managerID,string contractName)
        {
            IMailConfigurationService mailerService = new MailConfigurationService();
            MailConfiguration mailConfig = mailerService.GetMailConfiguration();
            Mailer mailer = new Mailer(mailConfig);

            IUserService userService = new UserService();
            User user = userService.Get(userID);
            User manager = userService.Get(managerID);

            mailer.SendMail("Signing Contract", "Successful sign contract: "+ contractName+ " with "+ manager.Name, new List<string>() { user.Email });
            mailer.SendMail("Signing Contract", "Successful sign contract: " + contractName + " with "+ user.Name, new List<string>() { manager.Email });
        }

        public ActionResult GeneratePDFByContracTypetId(int contractId)
        {
            if (contractId != 0)
            {
                IContractTypeService contractService = new ContractTypeService();
                var contractType = ContractTypeMapper.ToViewModel(contractService.GetById(contractId));
                byte[] uploadedFile = new byte[contractType.Data.InputStream.Length];
                contractType.Data.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                return File(uploadedFile, "application/pdf");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateUser(UserViewModel model)
        {
            IUserService userService = new UserService();
			User currentUser = userService.Get(model.Id);
			if (!string.IsNullOrEmpty(model.Password) && currentUser.PasswordHash != model.Password)
			{
				model.Password = new PasswordHasher().HashPassword(model.Password);
			}
			model.SecurityStamp = Guid.NewGuid().ToString();
			userService.Update(model.ToDbModel());
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
using Client.Mappers;
using Client.Models;
using DatabaseNamespace;
using DatabaseNamespace.Models;
using DbServices;
using DbServices.UserServices;
using Logger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Client.Controllers
{
	[Authorize(Roles = "Manager, Admin")]
	public class ContractUserController : Controller
	{
		// GET: ContractUser
		public ActionResult Index()
		{
			IUserService service = new UserService();
			List<UserViewModel> contractUsers = new List<UserViewModel>();
			try
			{
				if (!User.IsInRole(Roles.Admin.ToString()))
				{
					contractUsers = service.GetByUserID(User.Identity.GetUserId()).Select(c => UserMapper.ToViewModel(c)).ToList();
				}
				else
				{
					string roleId = service.GetRoleIdByName(Roles.User.ToString());
					contractUsers = service.GetAll().Where(c => c.Id != User.Identity.GetUserId() && c.Roles.FirstOrDefault()?.RoleId == roleId).Select(c => UserMapper.ToViewModel(c)).ToList();
				}
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
			}
			return View(contractUsers);
		}

		[HttpPost]
		public FileResult ExportToCsv()
		{
			try
			{
				Exporter exporter = new Exporter(DatabaseContext.connectionString);
				string currentDirectory = System.IO.Path.GetTempPath();
				string filename = "Table_" + DateTime.Now.ToString("yyyy-mm-dd_hh-MM-ss") + ".csv";
				FileInfo info = new FileInfo(Path.Combine(currentDirectory, filename));

				string query = String.Format("SELECT * FROM AspNetUsers WHERE UserId = '{0}'", User.Identity.GetUserId());
				bool result = exporter.SqlToCsv(query, info.FullName);

				Log.Write(String.Format("User: {0} exported users to csv ({1}).", User.Identity.GetUserName(), filename), LogLevel.Info, User.Identity.GetUserId());

				byte[] fileBytes = System.IO.File.ReadAllBytes(info.FullName);
				return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
				throw ex;
			}
		}

		[HttpPost]
		public FileResult ExportUsers()
		{
			try
			{
				HttpContext.Server.ScriptTimeout = 300;

				Exporter exporter = new Exporter(DatabaseContext.connectionString);
				string currentDirectory = System.IO.Path.GetTempPath();
				string filename = "Export_" + DateTime.Now.ToString("yyyy-mm-dd_hh-MM-ss") + ".bacpac";
				FileInfo info = new FileInfo(Path.Combine(currentDirectory, filename));

				exporter.ExportAzureDatabase(info.FullName);

				if (info.Exists)
				{
					Log.Write(String.Format("User: {0} backed up database to {1}.", User.Identity.GetUserName(), filename), LogLevel.Info, User.Identity.GetUserId());

					byte[] fileBytes = System.IO.File.ReadAllBytes(info.FullName);
					return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
				}
				else
				{
					throw new FileNotFoundException();
				}
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
				throw ex;
			}

		}

		// GET: ContractUser/Details/5
		public ActionResult Details(string id)
		{
			UserViewModel contractUser = null;
			try
			{
				IUserService userService = new UserService();
				IContractService contracService = new ContractService();
				contractUser = UserMapper.ToViewModel(userService.Get(id));
				if (User.IsInRole(Roles.User.ToString()))
				{
					contractUser.UserContracts = contracService.GetContractsByUserID(id).Select(c => ContractMapper.ToViewModel(c)).ToList();
				}
				else
				{
					contractUser.UserContracts = contracService.GetContractsByUserAndManager(id, User.Identity.GetUserId()).Select(c => ContractMapper.ToViewModel(c)).ToList();
				}
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
			}
			return View(contractUser);
		}

		// GET: ContractUser/Create
		public ActionResult Create()
		{
			return PartialView("_Add", new UserViewModel() { Rolename = Roles.User.ToString() });
		}

		// POST: ContractUser/Create
		[HttpPost]
		public ActionResult Create(UserViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					IUserService service = new UserService();
					User user = model.ToDbModel();
					user.UserId = User.Identity.GetUserId();
					var contracttype = service.Insert(user);

					Log.Write(String.Format("Manager: {0} created user {1}.", User.Identity.GetUserName(), model.Name), LogLevel.Info, User.Identity.GetUserId());

					return RedirectToAction("Index");
				}
				return PartialView("_Add", model);

			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
				return PartialView("_Add", model);
			}
		}

		// GET: ContractUser/Edit/5
		public ActionResult Edit(string id)
		{
			IUserService service = new UserService();
			UserViewModel model = UserMapper.ToViewModel(service.Get(id));
			model.Rolename = Roles.User.ToString();
			return PartialView("_Edit", model);
		}

		// POST: ContractUser/Edit/5
		[HttpPost]
		public ActionResult Edit(UserViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					IUserService service = new UserService();
					User currentUser = service.Get(model.Id);
					if (!string.IsNullOrEmpty(model.Password) && currentUser.PasswordHash != model.Password)
					{
						model.Password = new PasswordHasher().HashPassword(model.Password);
					}
					model.SecurityStamp = Guid.NewGuid().ToString();
					User user = model.ToDbModel();
					user.UserId = User.Identity.GetUserId();
					var contracttype = service.Update(user);

					Log.Write(String.Format("Manager: {0} edited user {1}.", User.Identity.GetUserName(), model.Name), LogLevel.Info, User.Identity.GetUserId());
					return Json(new { success = true });
				}

				return PartialView("_Edit", model);
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
				return PartialView("_Edit", model);
			}
		}

		// GET: ContractUser/Delete/5
		public ActionResult Delete(string Id)
		{
			IUserService contractService = new UserService();
			bool result = contractService.Delete(Id);

			Log.Write(String.Format("Manager: {0} removed user {1}.", User.Identity.GetUserName(), Id), LogLevel.Info, User.Identity.GetUserId());

			return Json(result, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SendContract(string userId)
		{
			try
			{
				IContractTypeService contractTypeService = new ContractTypeService();
				List<ContractTypeViewModel> contractTypeList = contractTypeService.GetContractTypesByUser(User.Identity.GetUserId()).Select(c => ContractTypeMapper.ToViewModel(c, false)).ToList();

				SendContractViewModel model = new SendContractViewModel()
				{
					UserId = userId,
					ListContractTypes = contractTypeList
				};
				return PartialView("_SendContract", model);
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
				throw ex;
			}
		}
		[HttpGet]
		public ActionResult Send(int selectedContractType, string userId)
		{
			try
			{
                string link = "http://gdprclient.azurewebsites.net/Contract/Details?contractId=";
                //string link = "http://localhost:1738/Contract/Details?contractId=";
                var currentManagerId = User.Identity.GetUserId();			
				Contract contract = new Contract()
				{
					Id = Guid.NewGuid().ToString(),
					InviteDate = DateTime.Now,
					Status = ContractStatus.NotSigned,
					ManagerID = currentManagerId,
					UserID = userId,
					ContractTypeId = selectedContractType
				};
                Contract newContract=SaveContract(contract);
				            
                if (newContract != null)
				{
                    string url = String.Format("{0}{1}", link, newContract.Id);
                    Request request = new Request()
                    {
                        ContractId = newContract.Id,
                        DateRequested = DateTime.Now,
                        ValidUntil = DateTime.Now.AddDays(30),
                        Url = url
                    };
                    SaveRequest(request);                  
                    SendMail(userId, url);               
				}
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);				
			}
			return RedirectToAction("Index", "ContractUser");
        }

        private Contract SaveContract(Contract contract)
        {
            IContractService contractService = new ContractService();
            return contractService.Insert(contract);
        }

        private void SaveRequest(Request request)
        {
            IRequestService reqestService = new RequestService();
            reqestService.InsertRequest(request);
        }

        private void SendMail(string userId,string url)
        {
            IMailConfigurationService mailerService = new MailConfigurationService();
            MailConfiguration mailConfig = mailerService.GetMailConfiguration();
            Mailer mailer = new Mailer(mailConfig);

            IUserService userService = new UserService();
            User user = userService.Get(userId);

            mailer.SendMail("Signing Contract", "Please go to this link to sign the contract: "+ url, new List<string>() { user.Email });
        }

		public JsonResult GetContractTypes()
		{
			var currentManagerId = User.Identity.GetUserId();
			using (DatabaseContext db = new DatabaseContext())
			{
				var resultData = db.ContractTypes.Select(c => new { Value = c.ID, Text = c.Name, ManagerId = c.ManagerId }).Where(c => c.ManagerId == currentManagerId).ToList();
				return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
			}

		}

	}
}

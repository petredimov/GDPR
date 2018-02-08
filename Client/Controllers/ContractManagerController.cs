using Client.Mappers;
using Client.Models;
using DatabaseNamespace;
using DbServices.UserServices;
using Logger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ContractManagerController : Controller
    {	

		// GET: ContractManager
		public ActionResult Index()
        {
            IUserService managerService = new UserService();
			string roleId = managerService.GetRoleIdByName(Roles.Manager.ToString());
			List<User> managers = managerService.GetByUserID(User.Identity.GetUserId()).Where(c=>c.Roles.FirstOrDefault()?.RoleId == roleId).ToList();			
            return View(managers.Select(c => UserMapper.ToViewModel(c)).ToList());
        }

		public ActionResult Details(string id)
		{
			IUserService managerService = new UserService();
            User manager = managerService.Get(id);
            UserViewModel model = UserMapper.ToViewModel(manager);
            return View(model);
		}

		public ActionResult Create()
		{
			return PartialView("_Add", new UserViewModel() { Rolename = Roles.Manager.ToString() });
		}

		[HttpPost]
		public ActionResult Create(UserViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					IUserService service = new UserService();
					var md5 = new MD5CryptoServiceProvider();
					var md5data = md5.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
					model.Password = Encoding.UTF8.GetString(md5data);
					var contractManager = service.Insert(model.ToDbModel());

					Log.Write(String.Format("Manager: {0} created user: {1}.", User.Identity.GetUserName(), model.Name), LogLevel.Info, User.Identity.GetUserId());

					return RedirectToAction("Index");
				}

				return PartialView("_Add", model);
			}
			catch(Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);
				return PartialView("_Add", model);
			}
		}

		public ActionResult Edit(string id)
		{
			IUserService service = new UserService();
			UserViewModel model = UserMapper.ToViewModel(service.Get(id));
			model.Rolename = Roles.Manager.ToString();
			return PartialView("_Edit", model);
		}

		// POST: ContractType/Edit/5
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
					var contractManager = service.Update(user);

					Log.Write(String.Format("Manager: {0} edited user: {1}.", User.Identity.GetUserName(), model.Name), LogLevel.Info, User.Identity.GetUserId());
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

		// GET: ContractType/Delete/5
		public ActionResult Delete(string Id)
		{
			bool result = false;

			try
			{
				IUserService contractService = new UserService();
				result = contractService.Delete(Id);

				Log.Write(String.Format("Manager: {0} removed user: {1}.", User.Identity.GetUserName(), Id), LogLevel.Info, User.Identity.GetUserId());
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);				
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}	
	}
}
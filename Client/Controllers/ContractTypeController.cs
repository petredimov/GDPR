using Client.Mappers;
using Client.Models;
using DbServices;
using Logger;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    [Authorize(Roles = "Manager, Admin")]
    public class ContractTypeController : Controller
    {
        // GET: ContractType
        public ActionResult Index()
        {
            IContractTypeService service = new ContractTypeService();

			List<ContractTypeViewModel> contractTypes = new List<ContractTypeViewModel>();

			if (User.IsInRole("Admin"))
			{
				contractTypes = service.GetAll().Select(c => ContractTypeMapper.ToViewModel(c, false)).ToList();
			}
			else
			{
				contractTypes = service.GetContractTypesByUser(User.Identity.GetUserId()).Select(c => ContractTypeMapper.ToViewModel(c, false)).ToList();
			}
            return View(contractTypes);
        }

		public ActionResult Remove(int id)
        {
            IContractTypeService contractService = new ContractTypeService();
            var isDeleted = contractService.Delete(id);

            Log.Write(String.Format("User: {0} removed contract: {1}", User.Identity.GetUserName(), id));

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        // GET: ContractType/Details/5
        public ActionResult Details(int id)
        {
            IContractTypeService contractService = new ContractTypeService();
            var contractType = ContractTypeMapper.ToViewModel(contractService.GetById(id));
            return View(contractType);
        }

        public ActionResult GeneratePDF(ContractTypeViewModel contractTypeViewModel)
        {
            if (contractTypeViewModel != null)
            {
                IContractTypeService contractService = new ContractTypeService();
                var contractType = ContractTypeMapper.ToViewModel(contractService.GetById(contractTypeViewModel.ID));
                byte[] uploadedFile = new byte[contractType.Data.InputStream.Length];
                contractType.Data.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                return File(uploadedFile, "application/pdf");
            }
            return View();
        }

        // GET: ContractType/Create
        public ActionResult Create()
        {
            return PartialView("_Add", new ContractTypeViewModel());
        }

        // POST: ContractType/Create
        [HttpPost]
        public ActionResult Create(ContractTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IContractTypeService service = new ContractTypeService();
					model.ManagerId = User.Identity.GetUserId();
					var contracttype = service.Insert(model.ToDbModel());

					Log.Write(String.Format("User: {0} created contract: {1}", User.Identity.GetUserName(), model.Name));
				}
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ContractType/Edit/5
        public ActionResult Edit(int id)
        {
            IContractTypeService contractTypeService = new ContractTypeService();
            ContractTypeViewModel model = ContractTypeMapper.ToViewModel(contractTypeService.GetById(id));
            return PartialView("_Edit", model);
        }
        [HttpPost]
        public ActionResult Edit(ContractTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IContractTypeService service = new ContractTypeService();
                    var contracttype = service.Update(model.ToDbModel());

					Log.Write(String.Format("User: {0} edited contract: {1}", User.Identity.GetUserName(), model.Name));
				}

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

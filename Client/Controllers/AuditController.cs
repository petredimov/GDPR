using Client.Mappers;
using Client.Models;
using DbServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
	[Authorize(Roles = "Manager, Admin")]
	public class AuditController : Controller
    {
        // GET: Audit
        public ActionResult Index()
        {
			List<AuditViewModel> auditViewModel = new List<AuditViewModel>();

			IAuditService auditService = new AuditService();

			if (!String.IsNullOrEmpty(User.Identity.GetUserId()))
			{
				if (User.IsInRole("Admin"))
				{
					auditViewModel = auditService.GetAll().Select(c => AuditMapper.ToViewModel(c)).OrderByDescending(c => c.Date).ToList();
				}
				else
				{
					auditViewModel = auditService.GetByUser(User.Identity.GetUserId()).Select(c => AuditMapper.ToViewModel(c)).OrderByDescending(c => c.Date).ToList();
				}
				return View(auditViewModel);
			}
			return RedirectToAction("Login", "Account");
        }
    }
}
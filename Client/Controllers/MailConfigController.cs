using Client.Models;
using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbServices;
using Microsoft.AspNet.Identity;
using Client.Mappers;
using Logger;

namespace Client.Controllers
{
	[Authorize(Roles ="Admin")]
    public class MailConfigController : Controller
    {
        // GET: MailConfig
        public ActionResult Index()
        {
			IMailConfigurationService service = new MailConfigurationService();
			var configuration = MailConfigMapper.ToViewModel(service.GetMailConfiguration());		

			return View(configuration);
        }

		[HttpPost]
		public ActionResult Save(MailConfigViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					model.UserId = User.Identity.GetUserId();
					IMailConfigurationService service = new MailConfigurationService();
					var mailConfig = service.GetMailConfiguration();
					if (mailConfig != null)
					{
						model.Id = mailConfig.Id;
						service.Update(model.ToDbModel());
					}
					else
					{
						service.Add(model.ToDbModel());
					}
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				Log.Write("", LogLevel.Error, User.Identity.GetUserId(), ex);				
			}
			return View(model);
		}
	}
}
using Client.Models;
using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Mappers
{
	public class MailConfigMapper
	{
		public static MailConfigViewModel ToViewModel(MailConfiguration model)
		{
			if (model != null)
			{
				return new MailConfigViewModel
				{
					Id = model.Id,
					EnableSsl = model.EnableSsl,
					From = model.From,
					Host = model.Host,
					MailType = model.MailType,
					Password = model.Password,
					Port = model.Port,
					Username = model.Username,
					UserId = model.UserId
				};
			}
			return null;
		}
	}
}
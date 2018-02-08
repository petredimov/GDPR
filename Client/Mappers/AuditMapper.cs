using Client.Models;
using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logger;

namespace Client.Mappers
{
	public class AuditMapper
	{
		public static AuditViewModel ToViewModel(Audit audit)
		{
			return new AuditViewModel
			{
				Id = audit.Id,
				Action = (LogLevel)audit.Action,
				Date = audit.Date,
				Message = audit.Message,
				UserId = audit.UserId
			};
		}
	}
}
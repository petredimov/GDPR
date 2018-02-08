using System;
using System.Collections.Generic;
using System.Text;
using DatabaseNamespace;
using System.Linq;

namespace DbServices
{
    public class AuditService : IAuditService
    {
		/// <summary>
		/// Get all Audits from database
		/// </summary>
		/// <returns></returns>
        public List<Audit> GetAll()
        {
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.Audits.ToList();
			}
        }


		/// <summary>
		/// Get Audits by user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
        public List<Audit> GetByUser(string userId)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                return context.Audits.Where(c => c.UserId == userId).ToList();
            }
        }


		/// <summary>
		/// Insert audit into database
		/// </summary>
		/// <param name="audit"></param>
		/// <returns></returns>
        public bool Insert(Audit audit)
        {
            using (DatabaseContext context = ContextFactory.GetContext())
            {
                context.Audits.Add(audit);
                return context.SaveChanges() > 0;
            }
        }
    }
}

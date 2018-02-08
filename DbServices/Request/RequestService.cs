using DatabaseNamespace;
using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
	public class RequestService : IRequestService
	{
		/// <summary>
		/// Get all requests from database
		/// </summary>
		/// <returns></returns>
		public List<Request> GetAllRequests()
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.Requests.ToList();
			}
		}

		/// <summary>
		/// Get all request for contract id
		/// </summary>
		/// <param name="contractId"></param>
		/// <returns>boolean</returns>
		public List<Request> GetRequestsByContractId(string contractId)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				return context.Requests.Where(c => c.ContractId == contractId).ToList();
			}
		}

		/// <summary>
		/// Insert request to database
		/// </summary>
		/// <param name="request"></param>
		public void InsertRequest(Request request)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				context.Requests.Add(request);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Delete request from database
		/// </summary>
		/// <param name="requestId"></param>
		public void RemoveRequest(int requestId)
		{
			using (DatabaseContext context = ContextFactory.GetContext())
			{
				Request request = context.Requests.FirstOrDefault(c => c.Id == requestId);
				if (request != null)
				{
					context.Requests.Remove(request);
					context.SaveChanges();
				}
			}
		}
	}
}

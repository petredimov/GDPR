using DatabaseNamespace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices
{
	public interface IRequestService
	{
		void InsertRequest(Request request);
		List<Request> GetAllRequests();
		List<Request> GetRequestsByContractId(string contractId);
		void RemoveRequest(int requestId);
	}
}

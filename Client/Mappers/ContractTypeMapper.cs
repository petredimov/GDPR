using Client.Models;
using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Mappers
{
    public class ContractTypeMapper
    {
        public static ContractTypeViewModel ToViewModel(ContractType contractType,bool withData=true)
        {
            if (contractType != null)
            {
                if (withData)
                {
                    if (contractType.Data != null)
                    {
                        HttpPostedFileBase objFile = (HttpPostedFileBase)new MemoryPostedFile(contractType.Data);
                        return new ContractTypeViewModel
                        {
                            ID = contractType.ID,
                            Data = objFile,
                            Name = contractType.Name,
                            ManagerId=contractType.ManagerId,
                            
                            

                        };
                    }
                }
                else
                {
                    return new ContractTypeViewModel
                    {
                        ID = contractType.ID,
                        Name = contractType.Name,
                        ManagerId=contractType.ManagerId,
                    };
                }
            }
            
			
			return null;
        }
    }
}
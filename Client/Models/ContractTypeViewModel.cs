using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class ContractTypeViewModel
    {
        public int ID { get; set; }

		[DisplayName("Name")]
		[Required]
		public string Name { get; set; }

        [Required]
        public HttpPostedFileBase Data { get; set; }
        public string ManagerId { get; set; }

        public ContractType ToDbModel()
        {
            byte[] uploadedFile;
            if (this.Data!=null)
            {
                uploadedFile = new byte[this.Data.InputStream.Length];
                this.Data.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                return new ContractType
                {
                    ID = this.ID,
                    Data = uploadedFile,
                    Name = this.Name,
                    ManagerId=this.ManagerId,
                };
            }
            return null;
        }
    }
}
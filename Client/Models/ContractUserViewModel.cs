using DatabaseNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class ContractUserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Wrong format")]
        public string Email { get; set; }
        public List<ContractViewModel> Contracts { get; set; }

        public string Address { get; set; }
        public string CompanyName { get; set; }
        public UserViewModel Manager { get; set; }

        public ContractUser ToDbModel()
        {
            return new ContractUser
            {
                Id = this.Id,
                Address = this.Address,
                CompanyName = this.CompanyName,
                Contracts = this.Contracts?.Select(c => c.ToDbModel()).ToList(),
                Email = this.Email,
                Manager = this.Manager?.ToDbModel(),
                Name = this.Name
            };
        }
    }
}
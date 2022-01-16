using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class Customers
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string AuthorizedName { get; set; }
        [Required]


        public Guid BusinessId { get; set; }
        [ForeignKey("BusinessId")]
        public Business Business { get; set; }//Firma Sektörü ÖRN: Mühendislik, Tekstil vb..



        [ForeignKey("CompanyTypeId")]
        public Guid CompanyTypeId { get; set; }
        public CompanyType CompanyType { get; set; }


        public string Email { get; set; }
        public string Phone { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string DocumentsUrls { get; set; }//may needs Document Model.
    }
}

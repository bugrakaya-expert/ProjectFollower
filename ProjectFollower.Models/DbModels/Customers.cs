using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class Customers
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AuthorizedName { get; set; }
        public int Business { get; set; }//Firma Sektörü ÖRN: Mühendislik, Tekstil vb..
        public int CompanyType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string DocumentsUrls { get; set; }//may needs Document Model.
    }
}

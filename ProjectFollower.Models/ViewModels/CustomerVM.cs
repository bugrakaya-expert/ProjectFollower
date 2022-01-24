using Microsoft.AspNetCore.Http;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class CustomerVM : Customers
    {
        public IEnumerable<CompanyDocuments> Documents { get; set; }
        public IEnumerable<IFormFileCollection> Files { get; set; }
        public string Nullval { get; set; }
    }
}

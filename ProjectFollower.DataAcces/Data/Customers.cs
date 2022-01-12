using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.Data
{
    public class Customers
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AuthorizedName { get; set; }
        public int Business { get; set; }
        public int CompanyType { get; set; }

    }
}

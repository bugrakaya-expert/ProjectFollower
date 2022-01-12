using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string AppUserName { get; set; }
        public string IdentityNumber { get; set; }
    }
}

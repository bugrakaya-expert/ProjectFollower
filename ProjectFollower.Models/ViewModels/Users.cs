using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class Users
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string AppUserName { get; set; }
        public string IdentityNumber { get; set; }
        public string ImageUrl { get; set; }
        public Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}

using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class DepartmentsVM : Department
    {
        /*
        [ForeignKey("ApplicationUserId")]
        public Guid ApplicationUserId { get; set; }*/
        public IEnumerable<ApplicationUser> ApplicationUser { get; set; }
        public IEnumerable<ResponsibleUsers> ResponsibleUserSelected { get; set; }
    }
}

using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class ProjectVM : Projects
    {
        //public IEnumerable<Customers> Customers { get; set; }
        //public IEnumerable<ProjectTasks> ProjectTasks { get; set; }
        public ResponsibleUsers ResponsibleUsers { get; set; }
        public List<DepartmentsVM> DepartmentsVMs { get; set; }
        //public List<ProjectTasks> ProjectTasks { get; set; }
        
        [Required(ErrorMessage = "Proje üyelerine en az 1 kişi seçmelisiniz.")]
        public string[] UserId { get; set; }

        public string[] TaskDesc { get; set; }
    }
}

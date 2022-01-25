using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class ProjectVM : Projects
    {
        public IEnumerable<Customers> Customers { get; set; }
        public IEnumerable<ProjectTasks> ProjectTasks { get; set; }
    }
}

using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class ProjectListVM
    {
        public List<Projects> Projects {get;set;}
        
        public int DelayedProjects { get; set; }
    }
}

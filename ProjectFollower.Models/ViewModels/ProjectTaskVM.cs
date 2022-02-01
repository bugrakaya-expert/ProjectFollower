using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class ProjectTaskVM
    {
        public Guid ProjectId { get; set; }
        public List<ProjectTasks> ProjectTasks { get; set; }
    }
}

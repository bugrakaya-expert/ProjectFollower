using ProjectFollower.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ProjectTasks
    {
        public Guid Id { get; set; }

        [ForeignKey("ProjectsId")]
        public Guid ProjectsId { get; set; }
        public Projects Projects { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }

        [NotMapped]
        public List<TaskPlayers> TaskPlayers { get; set; }
        
    }
}

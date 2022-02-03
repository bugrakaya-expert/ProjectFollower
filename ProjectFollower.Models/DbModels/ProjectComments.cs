using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ProjectComments
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("ProjectsId")]
        public Guid ProjectsId { get; set; }
        public Projects Projects { get; set; }
        public DateTime CommentTime { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Comment { get; set; }
    }
}

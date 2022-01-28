using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ProjectDocuments
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("ProjectsId")]
        public Guid ProjectsId { get; set; }
        public Projects Projects { get; set; }
        public string FileName { get; set; }
    }
}

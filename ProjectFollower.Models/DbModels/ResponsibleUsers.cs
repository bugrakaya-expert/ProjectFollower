using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ResponsibleUsers
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public string Title { get; set; }
        public Guid UserId { get; set; }
    }
}

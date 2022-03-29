using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ResponsibleMeetings
    {
        [Key]
        public Guid Id { get; set; }
        public string MeetingId { get; set; }
        public string UserId { get; set; }

        [NotMapped]
        public string Text { get; set; }
    }
}

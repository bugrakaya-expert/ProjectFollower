using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class MeetingsVM
    {
        public IEnumerable<Meetings> Meetings { get; set; }
        public IEnumerable<CustomerMeetingVM> Customer { get; set; }
        public IEnumerable<ResponsibleMeetings> ResponsibleMeetings { get; set; }
    }
}

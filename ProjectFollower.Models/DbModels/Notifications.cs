using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class Notifications
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get;set; }
        public DateTime Created { get; set; }
        public bool Read { get; set; }
    }
}

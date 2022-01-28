using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class SchedulerVM
    {
        public IEnumerable<Scheduler> Scheduler { get; set; }
        public IEnumerable<SchedulerPriority> SchedulerPriority { get; set; }
        public IEnumerable<SchedulerCustomerVM> Customers { get; set; }
    }
}

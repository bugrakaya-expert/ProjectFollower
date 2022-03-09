using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class TaskPlayersVM
    {
        public string UserId { get; set; }
        public string ProjectTaskId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

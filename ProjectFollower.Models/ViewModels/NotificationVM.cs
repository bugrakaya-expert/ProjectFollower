using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public  class NotificationVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class EditUserPass
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmnewPassword { get; set; }
    }
}

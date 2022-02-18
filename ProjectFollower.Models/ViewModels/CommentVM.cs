using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class CommentVM
    {
        //public ProjectComments ProjectComments { get; set; }
        public Guid Id { get; set; }
        public string ProjectsId { get; set; }
        //public string ApplicationUserId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Img { get; set; }
        public string Comment { get; set; }
        public string CommentTime { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string AppUserName { get; set; }
        public string IdentityNumber { get; set; }

        public string ImageUrl { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public string UserRole { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}

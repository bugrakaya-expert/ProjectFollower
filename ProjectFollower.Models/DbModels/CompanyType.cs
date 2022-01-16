using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class CompanyType
    {
        [Key]
        public Guid MyProperty { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

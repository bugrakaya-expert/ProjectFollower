using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class CompanyDocuments
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("CustomerId")]
        public Guid CustomerId { get; set; }
        public string DocumentUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class Projects
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("CustomersId")]
        public Guid CustomersId { get; set; }
        public Customers Customers { get; set; }
        public string CreationDate { get; set; }
        public string EndingDate { get; set; }
        public string FinishDate { get; set; }

    }
}

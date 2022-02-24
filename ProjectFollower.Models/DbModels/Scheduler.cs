using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class Scheduler
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        //public DateTime EndDate { get; set; }
        [ForeignKey("CustomersId")]
        public Guid CustomersId { get; set; }
        public Customers Customers { get; set; }
        public int PriorityId { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public bool AllDay { get; set; }
        public Scheduler()
        {
            AllDay = true;
        }


    }
}

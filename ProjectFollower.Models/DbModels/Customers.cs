using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class Customers
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Bir firma adı girmelisiniz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bir firma yetkilisi adı adresi girmelisiniz.")]
        public string AuthorizedName { get; set; }
        [Required(ErrorMessage = "Bir firma e-posta adresi girmelisiniz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bir firma telefon numarası girmelisiniz.")]
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool SchedulerEnabled { get; set; }
    }
}

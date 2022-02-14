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
        [Required(ErrorMessage = "Bir Proje adı girmelisiniz.")]
        [StringLength(48, ErrorMessage = "Proje adı en fazla {1} karakterle sınırlıdır ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Firma seçmek zorunludur.")]
        [ForeignKey("CustomersId")]
        public Guid CustomersId { get; set; }
        public Customers Customers { get; set; }
        public string CreationDate { get; set; }
        [Required(ErrorMessage = "Öngörülen Bitiş Tarihi seçmelisiniz.")]
        public string EndingDate { get; set; }
        public string FinishDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public bool Archived { get; set; }

        [NotMapped]
        public int ProjectSequence { get; set; }
        [NotMapped]
        public int SequanceDate { get; set; }
        [NotMapped]
        public bool IsDelayed { get; set; }

    }
}

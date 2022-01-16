using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class UserRegisterVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrenizi belirlemek zorundasınız.")]
        [StringLength(100, ErrorMessage = "Şifreniz en az {2} en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }
        /*
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Şifreniz Tekrarı ile eşleşmiyor.")]
        public string ConfirmPassword { get; set; }*/

        public string UserName { get; set; }
        /*
        [Required]
        public string PhoneNumber { get; set; }*/
        [Required(ErrorMessage = "Ad Soyad belirlemek zorunludur.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Ad Soyad belirlemek zorunludur.")]
        public string Lastname { get; set; }
        public string AppUserName { get; set; }
        public string IdentityNumber { get; set; }
        public Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}

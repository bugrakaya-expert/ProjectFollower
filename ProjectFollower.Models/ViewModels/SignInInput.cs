using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class SignInInput
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

        public bool RememberMe { get; set; }

    }
}

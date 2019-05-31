using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WTCPortal.Models
{
    public class User : Person
    {
        // public bool IsEmailVerified { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        [Display(Name = "Password")]
        public string Password1 { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password1", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword1 { get; set; }

        public string Salt { get; set; }
        public string PassThe { get; set; }
    }

    public partial class UserLogin
    {
        [Key]
        public int id { get; set; }
        public bool RememberMe { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
    }

}
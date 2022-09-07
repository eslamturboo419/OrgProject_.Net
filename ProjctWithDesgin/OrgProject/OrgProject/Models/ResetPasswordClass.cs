using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrgProject.Models
{
    public class ResetPasswordClass
    {
        public string HiddenString { get; set; }

        [Required(ErrorMessage = "Old Password Is Required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }




        [Required(ErrorMessage = "New Password Is Required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "This Password Dosn't Mastch The Same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }



    }
}
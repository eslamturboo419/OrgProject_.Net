using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrgProject.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Required Email")]
        // [DataType(DataType.EmailAddress,ErrorMessage ="This Is Not Email Address")]
        //[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Must Like That  ' joe@aol.com' ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }


    }
}
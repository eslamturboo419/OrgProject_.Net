using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DayNum1.Models
{
    public class GmailClass
    {

        [Required(ErrorMessage = "Required Name" ,AllowEmptyStrings =false) ]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required Email" ,AllowEmptyStrings =false)]
         [DataType(DataType.EmailAddress,ErrorMessage ="This Is Not Email Address")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Must Like That  ' joe@aol.com' ")]
        public string YourEmail { get; set; }


        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required Message" , AllowEmptyStrings =false )]
        public string Message { get; set; }



    }
}
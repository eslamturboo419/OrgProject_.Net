using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgProject.Models
{
    public class ReplyVM
    {

        public string Reply { get; set; }
        // comment id
        public int CommentID { get; set; }

        public int ProjectID { get; set; }

    }
}
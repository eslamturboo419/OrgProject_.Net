//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrgProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reply
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> Comment_id { get; set; }
        public Nullable<int> Project_Id { get; set; }
        public Nullable<int> Master_id { get; set; }
    
        public virtual Comment Comment { get; set; }
        public virtual FinalMaster FinalMaster { get; set; }
        public virtual FinalProject FinalProject { get; set; }
    }
}
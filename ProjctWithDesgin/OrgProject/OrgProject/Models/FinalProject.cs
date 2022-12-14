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
    using System.ComponentModel.DataAnnotations;

    public partial class FinalProject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinalProject()
        {
            this.Comments = new HashSet<Comment>();
            this.Replies = new HashSet<Reply>();
        }
    
        public int id { get; set; }

        [Required(ErrorMessage = "Leader Name Is Required")]
        [Display(Name = "Leader Name")]
        public string LeaderName { get; set; }

        [Required(ErrorMessage = "Name Of Project Is Required")]
        [Display(Name = "Name Of Project")]
        public string NameOfProject { get; set; }

        [Required(ErrorMessage = "Names Of Team Is Required")]
        [Display(Name = "Names Of Team ")]
        public string NmaesOfTeam { get; set; }

        [Required(ErrorMessage = "Descrpition Is Required")]
        public string Descrpition { get; set; }

        [Required(ErrorMessage = "Title Of Project Is Required")]
        [Display(Name = "Title Of Project")]
        public string TitleOfProject { get; set; }
        public Nullable<bool> IsActive { get; set; }

        [Display(Name = "Link Of Code")]
        public string SourceCode { get; set; }

        [Display(Name = "Video '.mp4' ")]
        public string VideoUpload { get; set; }

        [Display(Name = "Pdf File '.pdf' ")]
        [Required(ErrorMessage = "Pdf File Is Required")]
        public string PdfFile { get; set; }

        [Display(Name = "Image   ")]
        [Required(ErrorMessage = "Image Is Required")]
        public string ImageProject { get; set; }

        [Display(Name = "Year Of Project ")]
        public Nullable<int> YearOfTheProject { get; set; }

        [Display(Name = "Department Name")]
        public Nullable<int> Dep_id { get; set; }

        [Display(Name = "Doctor Name")]
        public Nullable<int> Doc_id { get; set; }

        [Display(Name = "teacher Assistant")]
        public Nullable<int> TeachAss_id { get; set; }
    
        public virtual AllUserData AllUserData { get; set; }
        public virtual AllUserData AllUserData1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Departement_Name Departement_Name { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reply> Replies { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CharitAble_current.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Cases
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Cases()
        {
            this.tbl_Donations = new HashSet<tbl_Donations>();
        }
    
        public int CaseID { get; set; }
        public Nullable<int> NGO_ID { get; set; }
        public string CaseTitle { get; set; }
        public Nullable<System.DateTime> PostedDate { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public Nullable<int> StatusID { get; set; }
    
        public virtual tbl_NGOMaster tbl_NGOMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Donations> tbl_Donations { get; set; }
    }
}

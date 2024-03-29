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
    
    public partial class tbl_DonorReplies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_DonorReplies()
        {
            this.tbl_Orders = new HashSet<tbl_Orders>();
        }
    
        public int ReplyID { get; set; }
        public int DonorID { get; set; }
        public int CaseID { get; set; }
        public byte Quantity { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public System.DateTime PostedDateTime { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string isActive { get; set; }
    
        public virtual tbl_Cases tbl_Cases { get; set; }
        public virtual tbl_DonorMaster tbl_DonorMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Orders> tbl_Orders { get; set; }
    }
}

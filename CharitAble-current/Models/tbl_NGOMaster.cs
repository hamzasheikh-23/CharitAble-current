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
    
    public partial class tbl_NGOMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_NGOMaster()
        {
            this.tbl_Cases = new HashSet<tbl_Cases>();
            this.tbl_Orders = new HashSet<tbl_Orders>();
            this.tbl_SuccessStories = new HashSet<tbl_SuccessStories>();
            this.tbl_PaymentInfo = new HashSet<tbl_PaymentInfo>();
            this.tbl_Orders1 = new HashSet<tbl_Orders>();
        }
    
        public int NGO_ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> PlanID { get; set; }
        public Nullable<int> PaymentInfoID { get; set; }
        public string TagLine { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> SubscriptionEndDate { get; set; }
        public Nullable<System.DateTime> SubscriptionStartDate { get; set; }
        public string isActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Cases> tbl_Cases { get; set; }
        public virtual tbl_Users tbl_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Orders> tbl_Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SuccessStories> tbl_SuccessStories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PaymentInfo> tbl_PaymentInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Orders> tbl_Orders1 { get; set; }
    }
}

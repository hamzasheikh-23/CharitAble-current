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
    
    public partial class tbl_SubscriptionPlan
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> AdminID { get; set; }
        public string Description { get; set; }
        public string isActive { get; set; }
    
        public virtual tbl_Admin tbl_Admin { get; set; }
    }
}

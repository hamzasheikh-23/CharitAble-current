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
        public int ReplyID { get; set; }
        public int DonorID { get; set; }
        public int CaseID { get; set; }
        public byte Quanitity { get; set; }
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
        public virtual tbl_Status tbl_Status { get; set; }
    }
}

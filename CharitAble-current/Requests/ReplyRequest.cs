using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class ReplyRequest
    {
        public int ReplyId { get; set; }
        public int DonorId { get; set; }
        public string DonorName { get; set; }
        public int CaseId { get; set; }
        public string CaseTitle { get; set; }
        public byte Quantity { get; set; }
        public int? UnitId { get; set; }
        public string Unit { get; set; }
        public int RemainingQuantity { get; set; }
        public int Sum { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public string IsActive { get; set; }
        public DateTime PostedDateTime { get; set; }
        public string Image1Base64 { get; set; }
        public string Image2Base64 { get; set; }
        public string Image3Base64 { get; set; }
        public string Image1Name { get; set; }
        public string Image2Name { get; set; }
        public string Image3Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int? NGOId { get; set; }
        public int? CaseId { get; set; }
        public int? ReplyId { get; set; }
        public int? PaymentId { get; set; }
        public string NGOName { get; set; }
        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? OrderDateTime { get; set; }
    }
}
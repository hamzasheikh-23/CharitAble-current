using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class InvoiceRequest
    {
        public int InvoiceId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public long? CardNumber { get; set; }
        public string CardholderName { get; set; }
    }
}
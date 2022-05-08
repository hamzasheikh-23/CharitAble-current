using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class PaymentInfoRequest
    {
        public int PaymentInfoId { get; set; }
        public int? NgoId { get; set; }
        public string CardholderName { get; set; }
        public long? CardNumber { get; set; }
        public byte? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public int? CVV { get; set; }

    }
}
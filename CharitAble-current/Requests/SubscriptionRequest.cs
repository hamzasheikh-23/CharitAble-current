using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class SubscriptionRequest
    {
        public int ngoID { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public DateTime? SubscribeDate { get; set; }


    }
}
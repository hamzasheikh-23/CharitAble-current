using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class ResponseRequest
    {
        public int ResponseId { get; set; }
        public int NgoId { get; set; }
        public string NgoName { get; set; }
        public int DonationId { get; set; }
        public string DonationTitle { get; set; }
        public string DonationImage1 { get; set; }
        public string DonationImage2 { get; set; }
        public string DonationImage3 { get; set; }
        public string Address { get; set; }
        public string PickupAddress { get; set; }
        public int? Quantity { get; set; }
        public int? DonationCategoryId { get; set; }
        public string DonationCategory { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public DateTime? PostedDateTime { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string isActive { get; set; }
    }
}
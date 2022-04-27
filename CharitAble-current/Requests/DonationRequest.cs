using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class DonationRequest
    {
        public int DonationId { get; set; }
        public int? DonorId { get; set; }
        public string Title { get; set; }
        public short? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public short? QuantityPerUnit { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? PostedDate { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string Description { get; set; }
        public int? Rating { get; set; }
        public int? ConditionId { get; set; }
        public string Condition { get; set; }
        public int? CategoryId { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public string Image1base64 { get; set; }
        public string Image2base64 { get; set; }
        public string Image3base64 { get; set; }
        public string Image1Name { get; set; }
        public string Image2Name { get; set; }
        public string Image3Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }

    }
}
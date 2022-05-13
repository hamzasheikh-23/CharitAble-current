using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class CaseRequest
    {
        public int CaseId { get; set; }
        public int? NGOId { get; set; }
        public int? UserId { get; set; }
        public string NGOName { get; set; }
        public string CaseTitle { get; set; }
        public string DonationCategory { get; set; }
        public int? CategoryId { get; set; }
        public short? Quantity { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string Unit { get; set; }
        public int? UnitId { get; set; }
        public DateTime? PostedDate { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageName { get; set; }
        public string CoverImage { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class DonationCategoryRequest
    {
        public int CategoryId { get; set; }
        public string DonationCategory { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class FeedbackRequest
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Feedback { get; set; }
        public DateTime PostedDateTime { get; set; }
    }
}
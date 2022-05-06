using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class StoryRequest
    {
        public int StoryId { get; set; }
        public int? NGOId { get; set; }
        public string NGOName { get; set; }
        public string StoryTitle { get; set; }
        public DateTime? PostedDate { get; set; }
        public string Description { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string isActive { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageName { get; set; }
        public string CoverImage { get; set; }
    }
}
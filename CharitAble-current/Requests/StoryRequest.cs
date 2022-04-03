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
        public int NGOId { get; set; }
        public string StoryTitle { get; set; }
        public DateTime PostedDate { get; set; }
        public string Description { get; set; }
        public Image Picture { get; set; }
    }
}
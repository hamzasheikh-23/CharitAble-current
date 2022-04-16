using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using CharitAble_current.Models;
using CharitAble_current.Requests;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("story")]
    public class StoryController : ApiController
    {
        private charitable_dbEntities1 dbx = new charitable_dbEntities1();

        // GET: Story
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetStory()
        {
            var storyList = dbx.tbl_SuccessStories.Select(x =>
                new StoryRequest()
                {
                    StoryId = x.StoryID,
                    NGOId = (int)x.NGO_ID,
                    StoryTitle = x.StoryTitle,
                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    //Picture = x.Picture,
                });

            return Json(storyList);
        }
    }
}
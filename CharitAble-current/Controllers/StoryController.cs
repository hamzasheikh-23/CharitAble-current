using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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
        charitable_dbEntities1 dbx = new charitable_dbEntities1();
        // POST: story/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(StoryRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting story failed"
                };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                var isActive = "true";

                tbl_SuccessStories story = new tbl_SuccessStories();
                story.NGO_ID = value.NGOId;
                story.StoryTitle = value.StoryTitle;
                story.PostedDate = value.PostedDate = DateTime.Now;
                story.Description = value.Description;
                story.StatusID = value.StatusID = statusId;
                story.isActive = value.isActive = isActive;

                if (!string.IsNullOrEmpty(value.ImageName))
                {
                    string imagePath = @"D:\fyp-frontend\src\serverImages\successStory" + value.ImageName;
                    FileInfo fi = new FileInfo(imagePath);
                    Guid obj = Guid.NewGuid();
                    imagePath = @"D:\fyp-frontend\src\serverImages\successStory" + obj.ToString() + fi.Extension;
                    var cleanerBase = value.ImageBase64.Substring(value.ImageBase64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath, Convert.FromBase64String(cleanerBase));
                    story.CoverImage = imagePath;
                }

                dbx.tbl_SuccessStories.AddOrUpdate(story);
                var result = dbx.SaveChanges();

                if (result != 0)
                {
                    ret = new
                    {
                        code = "1",
                        status = "story posted successfully"
                    };
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: story/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var order = dbx.tbl_SuccessStories.Select(x =>
                new StoryRequest()
                {
                    StoryId = x.StoryID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    StoryTitle = x.StoryTitle,
                    PostedDate = x.PostedDate,
                    Description = x.Description,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    isActive = x.isActive
                }).ToList();


                if (order.Any())
                {
                    return Ok(order);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

    }
}
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
                story.StatusID = value.StatusId = statusId;
                story.isActive = value.isActive = isActive;

                if (!string.IsNullOrEmpty(value.ImageName))
                {
                    string imagePath = @"D:\fyp-frontend\src\serverImages\successStory\" + value.ImageName;
                    FileInfo fi = new FileInfo(imagePath);
                    Guid obj = Guid.NewGuid();
                    imagePath = @"D:\fyp-frontend\src\serverImages\successStory\" + obj.ToString() + fi.Extension;
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

                var story = dbx.tbl_SuccessStories.Select(x =>
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
                    isActive = x.isActive,
                    CoverImage = x.CoverImage
                }).ToList();

                foreach (StoryRequest item in story)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath1 = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.ImageName = fi.Name;
                    }
                }
                if (story.Any())
                {
                    return Ok(story);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: story/get?{storyId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int storyId)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var story = dbx.tbl_SuccessStories.Select(x =>
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
                    isActive = x.isActive,
                    CoverImage = x.CoverImage
                }).Where(x => x.StoryId == storyId).ToList();

                foreach (StoryRequest item in story)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath1 = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.ImageName = fi.Name;
                    }
                }
                if (story.Any())
                {
                    return Ok(story);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: story/get?{storyId}&&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(string status)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                var story = dbx.tbl_SuccessStories.Select(x =>
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
                    isActive = x.isActive,
                    CoverImage = x.CoverImage
                }).Where(x => x.StatusId == statusId).ToList();

                foreach (StoryRequest item in story)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath1 = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.ImageName = fi.Name;
                    }
                }
                if (story.Any())
                {
                    return Ok(story);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: story/get?{storyId}&&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int storyId, string status)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                var story = dbx.tbl_SuccessStories.Select(x =>
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
                    isActive = x.isActive,
                    CoverImage = x.CoverImage
                }).Where(x => x.StoryId == storyId && x.StatusId == statusId).ToList();

                foreach (StoryRequest item in story)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath1 = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.ImageName = fi.Name;
                    }
                }
                if (story.Any())
                {
                    return Ok(story);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        public IHttpActionResult GetD(int storyId, string isActive)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };


                var story = dbx.tbl_SuccessStories.Select(x =>
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
                    isActive = x.isActive,
                    CoverImage = x.CoverImage
                }).Where(x => x.StoryId == storyId && x.isActive == isActive).ToList();

                foreach (StoryRequest item in story)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath1 = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.ImageName = fi.Name;
                    }
                }
                if (story.Any())
                {
                    return Ok(story);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: story/get?{storyId}&&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int storyId, string status, string isActive)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                var story = dbx.tbl_SuccessStories.Select(x =>
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
                    isActive = x.isActive,
                    CoverImage = x.CoverImage
                }).Where(x => x.StoryId == storyId && x.StatusId == statusId && x.isActive == isActive).ToList();

                foreach (StoryRequest item in story)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath1 = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.ImageName = fi.Name;
                    }
                }
                if (story.Any())
                {
                    return Ok(story);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //PUT: story/delete/{id}
        [HttpPut]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var storyIds = (from x in dbx.tbl_SuccessStories select x.StoryID).ToList();

                object ret = new { isSuccess = false };

                if (storyIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == "Deleted" || x.Status == "deleted"
                                    select x.StatusID).SingleOrDefault();

                    ReplyRequest reply = new ReplyRequest();

                    reply.StatusId = statusId;
                    reply.IsActive = "false";

                    var existingStory = dbx.tbl_SuccessStories.Where(x => x.StoryID == id).FirstOrDefault();

                    existingStory.StatusID = reply.StatusId;
                    existingStory.isActive = reply.IsActive;

                    dbx.tbl_SuccessStories.AddOrUpdate(existingStory);
                    var result = dbx.SaveChanges();

                    if (result > 0)
                    {
                        ret = new { isSuccess = true };
                        return Ok(ret);

                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");

            }
        }

        //PUT:story/edit/{id}

        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult UpdateDonation(int id, StoryRequest value)
        {
            try
            {
                var storyIds = (from x in dbx.tbl_SuccessStories select x.StoryID).ToList();

                if (storyIds.Contains(id))
                {

                    var existingStory = dbx.tbl_SuccessStories.Where(x => x.StoryID == id).FirstOrDefault();

                    existingStory.StoryTitle = value.StoryTitle;
                    existingStory.Description = value.Description;
                    existingStory.StatusID = (from x in dbx.tbl_Status
                                              where x.Status == "Pending" || x.Status == "pending"
                                              select x.StatusID).FirstOrDefault();
                    existingStory.isActive = value.isActive;

                    if (!string.IsNullOrEmpty(value.ImageName))
                    {
                        string imagePath = @"D:\fyp-frontend\src\serverImages\donorReplies\" + value.ImageName;
                        FileInfo fi = new FileInfo(imagePath);
                        Guid obj = Guid.NewGuid();
                        imagePath = @"D:\fyp-frontend\src\serverImages\donorReplies\" + obj.ToString() + fi.Extension;
                        var cleanerBase = value.ImageBase64.Substring(value.ImageBase64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath, Convert.FromBase64String(cleanerBase));
                        existingStory.CoverImage = imagePath;
                    }

                    dbx.tbl_SuccessStories.AddOrUpdate(existingStory);
                    dbx.SaveChanges();

                    return Ok("record updated successfully");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }
    }
}
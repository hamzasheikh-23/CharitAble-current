using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CharitAble_current.Models;
using System.IO;
using CharitAble_current.Requests;
using System.Data.Entity.Migrations;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("reply")]
    public class RepliesController : ApiController
    {
        charitable_dbEntities1 dbx = new charitable_dbEntities1();

        //POST: reply/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(ReplyRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting reply failed"
                };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                var isActive = "true";

                tbl_DonorReplies reply = new tbl_DonorReplies();
                reply.DonorID = value.DonorId;
                reply.CaseID = value.CaseId;
                reply.Quanitity = value.Quantity;
                reply.Address = value.Address;
                reply.Message = value.Message;
                reply.isActive = value.IsActive = isActive;
                reply.PostedDateTime = value.PostedDateTime = DateTime.Now;
                reply.StatusID = value.StatusId = statusId;

                if (!string.IsNullOrEmpty(value.Image1Name))
                {
                    string imagePath1 = @"D:\fyp-frontend\src\serverImages\donorReplies" + value.Image1Name;
                    FileInfo fi = new FileInfo(imagePath1);
                    Guid obj = Guid.NewGuid();
                    imagePath1 = @"D:\fyp-frontend\src\serverImages\donorReplies" + obj.ToString() + fi.Extension;
                    var cleanerBase1 = value.Image1Base64.Substring(value.Image1Base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                    reply.Image1 = imagePath1;
                }

                if (!string.IsNullOrEmpty(value.Image2Name))
                {
                    string imagePath2 = @"D:\fyp-frontend\src\serverImages\donorReplies" + value.Image2Name;
                    FileInfo fi = new FileInfo(imagePath2);
                    Guid obj = Guid.NewGuid();
                    imagePath2 = @"D:\fyp-frontend\src\serverImages\donorReplies" + obj.ToString() + fi.Extension;
                    var cleanerBase2 = value.Image2Base64.Substring(value.Image2Base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                    reply.Image2 = imagePath2;
                }

                if (!string.IsNullOrEmpty(value.Image3Name))
                {
                    string imagePath3 = @"D:\fyp-frontend\src\serverImages\donorReplies" + value.Image3Name;
                    FileInfo fi = new FileInfo(imagePath3);
                    Guid obj = Guid.NewGuid();
                    imagePath3 = @"D:\fyp-frontend\src\serverImages\donorReplies" + obj.ToString() + fi.Extension;
                    var cleanerBase3 = value.Image3Base64.Substring(value.Image3Base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath3, Convert.FromBase64String(cleanerBase3));
                    reply.Image3 = imagePath3;
                }

                dbx.tbl_DonorReplies.AddOrUpdate(reply);
                var result = dbx.SaveChanges();

                if (result != 0)
                {
                    ret = new
                    {
                        code = "1",
                        status = "Reply posted successfully"
                    };
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: case/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var reply = dbx.tbl_DonorReplies.Select(x =>
                new ReplyRequest()
                {
                    ReplyId = x.ReplyID,
                    DonorId = x.DonorID,
                    DonorName = (from d in dbx.tbl_DonorMaster
                                 join u in dbx.tbl_Users on d.UserID equals u.UserID
                                 where d.DonorID == x.DonorID
                                 select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    Quantity = x.Quanitity,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3
                }).ToList();

                foreach (ReplyRequest item in reply)
                {
                    if (!string.IsNullOrWhiteSpace(item.Image1))
                    {
                        string imagePath1 = item.Image1;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.Image1Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image2))
                    {
                        string image2Path = item.Image2;
                        FileInfo fi = new FileInfo(image2Path);
                        item.Image2Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image3))
                    {
                        string imagePath3 = item.Image3;
                        FileInfo fi = new FileInfo(imagePath3);
                        item.Image3Name = fi.Name;
                    }
                }

                if (reply.Any())
                {
                    return Ok(reply);
                }
                return BadRequest((string)ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int caseId)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var reply = dbx.tbl_DonorReplies.Select(x =>
                new ReplyRequest()
                {
                    ReplyId = x.ReplyID,
                    DonorId = x.DonorID,
                    DonorName = (from d in dbx.tbl_DonorMaster
                                 join u in dbx.tbl_Users on d.UserID equals u.UserID
                                 where d.DonorID == x.DonorID
                                 select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    Quantity = x.Quanitity,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3
                }).Where(x => x.CaseId == caseId).ToList();

                foreach (ReplyRequest item in reply)
                {
                    if (!string.IsNullOrWhiteSpace(item.Image1))
                    {
                        string imagePath1 = item.Image1;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.Image1Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image2))
                    {
                        string image2Path = item.Image2;
                        FileInfo fi = new FileInfo(image2Path);
                        item.Image2Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image3))
                    {
                        string imagePath3 = item.Image3;
                        FileInfo fi = new FileInfo(imagePath3);
                        item.Image3Name = fi.Name;
                    }
                }

                if (reply.Any())
                {
                    return Ok(reply);
                }
                return BadRequest((string)ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}&&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int caseId, string status)
        {
            try
            {
                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                object ret = new { code = 0, status = "unsuccesfull request" };

                var reply = dbx.tbl_DonorReplies.Select(x =>
                new ReplyRequest()
                {
                    ReplyId = x.ReplyID,
                    DonorId = x.DonorID,
                    DonorName = (from d in dbx.tbl_DonorMaster
                                 join u in dbx.tbl_Users on d.UserID equals u.UserID
                                 where d.DonorID == x.DonorID
                                 select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    Quantity = x.Quanitity,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3
                }).Where(x => x.CaseId == caseId && x.StatusId == statusId).ToList();

                foreach (ReplyRequest item in reply)
                {
                    if (!string.IsNullOrWhiteSpace(item.Image1))
                    {
                        string imagePath1 = item.Image1;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.Image1Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image2))
                    {
                        string image2Path = item.Image2;
                        FileInfo fi = new FileInfo(image2Path);
                        item.Image2Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image3))
                    {
                        string imagePath3 = item.Image3;
                        FileInfo fi = new FileInfo(imagePath3);
                        item.Image3Name = fi.Name;
                    }
                }

                if (reply.Any())
                {
                    return Ok(reply);
                }
                return BadRequest((string)ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}&&{isActive}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetR(int caseId, string isActive)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var reply = dbx.tbl_DonorReplies.Select(x =>
                new ReplyRequest()
                {
                    ReplyId = x.ReplyID,
                    DonorId = x.DonorID,
                    DonorName = (from d in dbx.tbl_DonorMaster
                                 join u in dbx.tbl_Users on d.UserID equals u.UserID
                                 where d.DonorID == x.DonorID
                                 select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    Quantity = x.Quanitity,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3
                }).Where(x => x.CaseId == caseId && x.IsActive == isActive).ToList();

                foreach (ReplyRequest item in reply)
                {
                    if (!string.IsNullOrWhiteSpace(item.Image1))
                    {
                        string imagePath1 = item.Image1;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.Image1Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image2))
                    {
                        string image2Path = item.Image2;
                        FileInfo fi = new FileInfo(image2Path);
                        item.Image2Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image3))
                    {
                        string imagePath3 = item.Image3;
                        FileInfo fi = new FileInfo(imagePath3);
                        item.Image3Name = fi.Name;
                    }
                }

                if (reply.Any())
                {
                    return Ok(reply);
                }
                return BadRequest((string)ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}&&{status}&&{isActive}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int caseId, string status, string isActive)
        {
            try
            {
                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                object ret = new { code = 0, status = "unsuccesfull request" };

                var reply = dbx.tbl_DonorReplies.Select(x =>
                new ReplyRequest()
                {
                    ReplyId = x.ReplyID,
                    DonorId = x.DonorID,
                    DonorName = (from d in dbx.tbl_DonorMaster
                                 join u in dbx.tbl_Users on d.UserID equals u.UserID
                                 where d.DonorID == x.DonorID
                                 select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    Quantity = x.Quanitity,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3
                }).Where(x => x.CaseId == caseId && x.StatusId == statusId && x.IsActive == isActive).ToList();

                foreach (ReplyRequest item in reply)
                {
                    if (!string.IsNullOrWhiteSpace(item.Image1))
                    {
                        string imagePath1 = item.Image1;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.Image1Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image2))
                    {
                        string image2Path = item.Image2;
                        FileInfo fi = new FileInfo(image2Path);
                        item.Image2Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image3))
                    {
                        string imagePath3 = item.Image3;
                        FileInfo fi = new FileInfo(imagePath3);
                        item.Image3Name = fi.Name;
                    }
                }

                if (reply.Any())
                {
                    return Ok(reply);
                }
                return BadRequest((string)ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //PUT: reply/delete/{id}
        [HttpPut]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var replyIds = (from x in dbx.tbl_DonorReplies select x.ReplyID).ToList();

                if (replyIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == "Deleted" || x.Status == "deleted"
                                    select x.StatusID).SingleOrDefault();

                    ReplyRequest reply = new ReplyRequest();

                    reply.StatusId = statusId;
                    reply.IsActive = "false";

                    var existingReply = dbx.tbl_DonorReplies.Where(x => x.ReplyID == id).FirstOrDefault();

                    existingReply.StatusID = reply.StatusId;
                    existingReply.isActive = reply.IsActive;

                    dbx.tbl_DonorReplies.AddOrUpdate(existingReply);
                    var result = dbx.SaveChanges();

                    if (result > 0)
                    {
                        return Ok("record deleted successfully");

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

        //PUT:reply/edit/{id}

        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult UpdateDonation(int id, ReplyRequest value)
        {
            try
            {
                var replyIds = (from x in dbx.tbl_DonorReplies select x.ReplyID).ToList();

                if (replyIds.Contains(id))
                {

                    var existingReply = dbx.tbl_DonorReplies.Where(x => x.ReplyID == id).FirstOrDefault();

                    existingReply.Quanitity = value.Quantity;
                    existingReply.Address = value.Address;
                    existingReply.Message = value.Message;
                    existingReply.StatusID = (from x in dbx.tbl_Status
                                              where x.Status == "Pending" || x.Status == "pending"
                                              select x.StatusID).FirstOrDefault();
                    existingReply.isActive = value.IsActive;

                    if (!string.IsNullOrEmpty(value.Image1Name))
                    {
                        string imagePath1 = @"D:\fyp-frontend\src\serverImages\donorReplies" + value.Image1Name;
                        FileInfo fi = new FileInfo(imagePath1);
                        Guid obj = Guid.NewGuid();
                        imagePath1 = @"D:\fyp-frontend\src\serverImages\donorReplies" + obj.ToString() + fi.Extension;
                        var cleanerBase1 = value.Image1Base64.Substring(value.Image1Base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                        existingReply.Image1 = imagePath1;
                    }

                    if (!string.IsNullOrEmpty(value.Image2Name))
                    {
                        string imagePath2 = @"D:\fyp-frontend\src\serverImages\donorReplies" + value.Image2Name;
                        FileInfo fi = new FileInfo(imagePath2);
                        Guid obj = Guid.NewGuid();
                        imagePath2 = @"D:\fyp-frontend\src\serverImages\donorReplies" + obj.ToString() + fi.Extension;
                        var cleanerBase2 = value.Image2Base64.Substring(value.Image2Base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                        existingReply.Image2 = imagePath2;
                    }

                    if (!string.IsNullOrEmpty(value.Image3Name))
                    {
                        string imagePath3 = @"D:\fyp-frontend\src\serverImages\donorReplies" + value.Image3Name;
                        FileInfo fi = new FileInfo(imagePath3);
                        Guid obj = Guid.NewGuid();
                        imagePath3 = @"D:\fyp-frontend\src\serverImages\donorReplies" + obj.ToString() + fi.Extension;
                        var cleanerBase3 = value.Image3Base64.Substring(value.Image3Base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath3, Convert.FromBase64String(cleanerBase3));
                        existingReply.Image3 = imagePath3;
                    }

                    dbx.tbl_DonorReplies.AddOrUpdate(existingReply);
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

        //GET:reply/remainingQuantity/{caseId}
        //[HttpGet]
        //[Route("remainingQuantity")]
        //public IHttpActionResult GetRemainingQuantity()
        //{
        //    var remainingQuantity = dbx.tbl_DonorReplies.Select(x => x.Quanitity).Sum().Where(x => x.CaseID == caseId);

        //    object ret = new { remainingQuantity, code = 1 };

        //    return Ok(ret);
        //}
    }
}
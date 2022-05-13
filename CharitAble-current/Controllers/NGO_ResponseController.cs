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
    [RoutePrefix("response")]
    public class NGO_ResponseController : ApiController
    {
        charitable_dbEntities2 dbx = new charitable_dbEntities2();

        //POST: response/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(ResponseRequest value)
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

                NGOResponse reply = new NGOResponse
                {
                    NGO_ID = value.NgoId,
                    DonationID = value.DonationId,
                    Address = value.Address,
                    Message = value.Message,
                    isActive = value.isActive = isActive,
                    PostedDateTime = value.PostedDateTime = DateTime.Now,
                    StatusID = value.StatusId = statusId
                };

                dbx.NGOResponses.AddOrUpdate(reply);
                var result = dbx.SaveChanges();

                if (result != 0)
                {
                    ret = new
                    {
                        code = "1",
                        status = "Response posted successfully"
                    };
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = dbx.NGOResponses.Select(x =>
                new ResponseRequest()
                {
                    ResponseId = x.ResponseID,
                    NgoId = x.NGO_ID,
                    NgoName = (from d in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on d.UserID equals u.UserID
                               where d.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    DonationId = x.DonationID,
                    DonationTitle = (from y in dbx.tbl_Donations
                                     where y.DonationID == x.DonationID
                                     select y.DonationTitle).FirstOrDefault(),
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    isActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                }).ToList();

                if (reply.Any())
                {
                    ret = new { reply, noData = true };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{donorId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetD(int donorId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = (from d in dbx.NGOResponses
                             join c in dbx.tbl_Donations on d.DonationID equals c.DonationID
                             where c.DonorID == donorId
                             select new ResponseRequest()
                             {
                                 ResponseId = d.ResponseID,
                                 NgoId = d.NGO_ID,
                                 NgoName = (from dm in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on dm.UserID equals u.UserID
                                            where dm.NGO_ID == d.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonationId = d.DonationID,
                                 DonationTitle = c.DonationTitle,
                                 Address = d.Address,
                                 Message = d.Message,
                                 StatusId = d.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == d.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 isActive = d.isActive,
                                 PostedDateTime = d.PostedDateTime,
                             }).ToList();


                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int donationId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = dbx.NGOResponses.Select(x =>
                new ResponseRequest()
                {
                    ResponseId = x.ResponseID,
                    NgoId = x.NGO_ID,
                    NgoName = (from dm in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on dm.UserID equals u.UserID
                               where dm.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    DonationId = x.DonationID,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    isActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                }).Where(x => x.DonationId == donationId).ToList();


                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{donorId}&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetR(int donorId, string status)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).FirstOrDefault();

                var reply = (from d in dbx.NGOResponses
                             join c in dbx.tbl_Donations on d.DonationID equals c.DonationID
                             where c.DonorID == donorId && d.StatusID == statusId
                             select new ResponseRequest()
                             {
                                 ResponseId = d.ResponseID,
                                 NgoId = d.NGO_ID,
                                 NgoName = (from dm in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on dm.UserID equals u.UserID
                                            where dm.NGO_ID == d.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonationId = d.DonationID,
                                 DonationTitle = c.DonationTitle,
                                 Address = d.Address,
                                 Message = d.Message,
                                 StatusId = d.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == d.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 isActive = d.isActive,
                                 PostedDateTime = d.PostedDateTime,
                             }).ToList();

                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{ngoId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetD(int donorId, int donationId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = (from d in dbx.NGOResponses
                             join c in dbx.tbl_Donations on d.DonationID equals c.DonationID
                             where c.DonorID == donorId && d.DonationID == donationId
                             select new ResponseRequest()
                             {
                                 ResponseId = d.ResponseID,
                                 NgoId = d.NGO_ID,
                                 NgoName = (from dm in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on dm.UserID equals u.UserID
                                            where dm.NGO_ID == d.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonationId = d.DonationID,
                                 DonationTitle = c.DonationTitle,
                                 Address = d.Address,
                                 Message = d.Message,
                                 StatusId = d.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == d.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 isActive = d.isActive,
                                 PostedDateTime = d.PostedDateTime,
                             }).ToList();

                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}&&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int donationId, string status)
        {
            try
            {
                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = dbx.NGOResponses.Select(x =>
                new ResponseRequest()
                {
                    ResponseId = x.ResponseID,
                    NgoId = x.NGO_ID,
                    NgoName = (from dm in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on dm.UserID equals u.UserID
                               where dm.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    DonationId = x.DonationID,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    isActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                }).Where(x => x.DonationId == donationId && x.StatusId == statusId).ToList();


                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}&&{isActive}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetReply(int donationId, string isActive)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = dbx.NGOResponses.Select(x =>
                new ResponseRequest()
                {
                    ResponseId = x.ResponseID,
                    NgoId = x.NGO_ID,
                    NgoName = (from dm in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on dm.UserID equals u.UserID
                               where dm.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    DonationId = x.DonationID,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    isActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                }).Where(x => x.DonationId == donationId && x.isActive == isActive).ToList();


                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{caseId}&&{status}&&{isActive}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int donationId, string status, string isActive)
        {
            try
            {
                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = dbx.NGOResponses.Select(x =>
                new ResponseRequest()
                {
                    ResponseId = x.ResponseID,
                    NgoId = x.NGO_ID,
                    NgoName = (from dm in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on dm.UserID equals u.UserID
                               where dm.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    DonationId = x.DonationID,
                    Address = x.Address,
                    Message = x.Message,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    isActive = x.isActive,
                    PostedDateTime = x.PostedDateTime,
                }).Where(x => x.DonationId == donationId && x.StatusId == statusId && x.isActive == isActive).ToList();


                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: reply/get?{ngoId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetD(int ngoId, string status)
        {
            try
            {
                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                object ret = new { noData = true, status = "unsuccesfull request" };

                var reply = (from d in dbx.NGOResponses
                             join c in dbx.tbl_Donations on d.DonationID equals c.DonationID
                             where d.NGO_ID == ngoId && d.StatusID == statusId
                             select new ResponseRequest()
                             {
                                 ResponseId = d.ResponseID,
                                 NgoId = d.NGO_ID,
                                 NgoName = (from dm in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on dm.UserID equals u.UserID
                                            where dm.NGO_ID == d.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonationId = d.DonationID,
                                 DonationTitle = c.DonationTitle,
                                 DonationImage1 = c.Image1,
                                 DonationImage2 = c.Image2,
                                 DonationImage3 = c.Image3,
                                 Address = d.Address,
                                 PickupAddress = c.Address,
                                 Quantity = c.Quantity,
                                 DonationCategoryId = c.Category,
                                 DonationCategory = (from x in dbx.tbl_DonationCategory
                                                     where x.CategoryID == c.Category
                                                     select x.DonationCategory).SingleOrDefault(),
                                 Message = d.Message,
                                 StatusId = d.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == d.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 isActive = d.isActive,
                                 PostedDateTime = d.PostedDateTime,
                             }).ToList();

                if (reply.Any())
                {
                    ret = new { reply, noData = false };
                    return Ok(ret);
                }
                return Json(ret);
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
                object ret = new { isSuccess = false };
                var replyIds = (from x in dbx.NGOResponses select x.ResponseID).ToList();

                if (replyIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == "Deleted" || x.Status == "deleted"
                                    select x.StatusID).SingleOrDefault();

                    ResponseRequest reply = new ResponseRequest
                    {
                        StatusId = statusId,
                        isActive = "false"
                    };

                    var existingReply = dbx.NGOResponses.Where(x => x.ResponseID == id).FirstOrDefault();

                    existingReply.StatusID = reply.StatusId;
                    existingReply.isActive = reply.isActive;

                    dbx.NGOResponses.AddOrUpdate(existingReply);
                    var result = dbx.SaveChanges();

                    if (result > 0)
                    {
                        ret = new { isSuccess = true, message = "record has been deleted" };
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

        //PUT:reply/edit/{id}

        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult Update(int id, ResponseRequest value)
        {
            try
            {
                var replyIds = (from x in dbx.NGOResponses select x.ResponseID).ToList();

                if (replyIds.Contains(id))
                {

                    var existingReply = dbx.NGOResponses.Where(x => x.ResponseID == id).FirstOrDefault();

                    existingReply.Address = value.Address;
                    existingReply.Message = value.Message;
                    existingReply.StatusID = (from x in dbx.tbl_Status
                                              where x.Status == "Pending" || x.Status == "pending"
                                              select x.StatusID).FirstOrDefault();
                    existingReply.isActive = value.isActive;



                    dbx.NGOResponses.AddOrUpdate(existingReply);
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


        //PUT: reply/edit?{id}&{status}

        [HttpPut]
        [Route("edit")]
        public IHttpActionResult UpdateReply(int id, string status)
        {
            try
            {
                var replyIds = (from x in dbx.NGOResponses select x.ResponseID).ToList();

                if (replyIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == status
                                    select x.StatusID).SingleOrDefault();

                    //var conditionId = (from x in dbx.tbl_DonationCondition
                    //                   where x.Condition == value.Condition
                    //                   select x.ConditionID).SingleOrDefault();

                    //var categoryId = (from x in dbx.tbl_DonationCategory
                    //                  where x.DonationCategory == value.Category
                    //                  select x.CategoryID).SingleOrDefault();

                    ResponseRequest value = new ResponseRequest();

                    var existingReply = dbx.NGOResponses.Where(x => x.ResponseID == id).FirstOrDefault();

                    existingReply.StatusID = value.StatusId = statusId;

                    dbx.NGOResponses.AddOrUpdate(existingReply);
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
                return BadRequest(ex + " : '" + ex.Message + "'");
            }
        }
    }
}
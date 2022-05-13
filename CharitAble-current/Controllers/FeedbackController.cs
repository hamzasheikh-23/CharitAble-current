using CharitAble_current.Models;
using CharitAble_current.Requests;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("feedback")]
    public class FeedbackController : ApiController
    {
        charitable_dbEntities2 dbx = new charitable_dbEntities2();
        // POST: feedback/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(FeedbackRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting feedback failed"
                };

                tbl_Feedback feedback = new tbl_Feedback();
                feedback.UserID = value.UserId;
                feedback.Feedback = value.Feedback;
                feedback.PostedDateTime = value.PostedDateTime;

                dbx.tbl_Feedback.AddOrUpdate(feedback);
                var result = dbx.SaveChanges();

                if (result != 0)
                {
                    ret = new
                    {
                        code = "1",
                        status = "Feedback posted successfully"
                    };
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: order/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var feedback = dbx.tbl_Feedback.Select(x =>
                new FeedbackRequest()
                {
                    FeedbackId = x.FeedbackID,
                    UserId = x.UserID,
                    UserName = (from u in dbx.tbl_Users
                                where u.UserID == x.UserID
                                select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    Feedback = x.Feedback,
                    PostedDateTime = x.PostedDateTime
                }).ToList();


                if (feedback.Any())
                {
                    return Ok(feedback);
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
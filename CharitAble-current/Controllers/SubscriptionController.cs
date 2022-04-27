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
    [RoutePrefix("subscription")]
    public class SubscriptionController : ApiController
    {

        private charitable_dbEntities1 dbx = new charitable_dbEntities1();


        // GET: Subscription
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                var subscription = dbx.tbl_SubscriptionPlan.Select(x =>
                new SubscriptionRequest()
                {
                    PlanId = x.PlanID,
                    PlanName = x.PlanName,
                    Amount = x.Amount,
                    Description = x.Description
                }).ToList();

                if (subscription.Count > 0)
                {
                    return Json(subscription);
                }
                else
                {
                    return BadRequest("Unable to retrieve data from database, check URL or other user input");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }


        [HttpPut]
        [Route("assign")]
        public IHttpActionResult Assign(int ngoId, SubscriptionRequest value)
        {
            try
            {
                var userIds = (from x in dbx.tbl_NGOMaster select x.UserID).ToList();

                if (userIds.Contains(ngoId))
                {

                    var existingNGO = dbx.tbl_NGOMaster.Where(x => x.NGO_ID == ngoId).FirstOrDefault();

                    existingNGO.PlanID = value.PlanId;
                    existingNGO.SubscriptionStartDate = DateTime.Now.Date;
                    existingNGO.SubscriptionEndDate = DateTime.Now.Date.AddMonths(1);

                    dbx.tbl_NGOMaster.AddOrUpdate(existingNGO);
                    dbx.SaveChanges();

                    return Json("Succesfully subscribed!!!");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }

        [HttpGet]
        [Route("validate")]
        public IHttpActionResult validate(int ngoId)
        {
            //var subscriptionStartDate = (from x in dbx.tbl_NGOMaster
            //                        where x.NGO_ID == ngoId
            //                        select x.SubscriptionEndDate).SingleOrDefault();

            object ret = new
            {
                code = 1,
                status = "subscription end, please pay bill to continue"
            };
            var subscriptionEndDate = (from x in dbx.tbl_NGOMaster
                                       where x.NGO_ID == ngoId
                                       select x.SubscriptionEndDate).SingleOrDefault();

            if (!(subscriptionEndDate < DateTime.Today.Date))
            {
                ret = new
                {
                    code = 2,
                    status = "You are subscribed right now"
                };

                return Ok(ret);
            }
            return Json(ret);
        }
    }
}

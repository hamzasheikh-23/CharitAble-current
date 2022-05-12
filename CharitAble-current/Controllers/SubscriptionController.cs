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

        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(SubscriptionRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting subscription plan failed"
                };


                var isActive = "true";

                tbl_SubscriptionPlan subs = new tbl_SubscriptionPlan
                {
                    PlanName = value.PlanName,
                    Amount = value.Amount,
                    Description = value.Description,
                    isActive = value.IsActive = isActive
                };

                dbx.tbl_SubscriptionPlan.AddOrUpdate(subs);
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
                    AdminId = x.AdminID,
                    PlanId = x.PlanID,
                    PlanName = x.PlanName,
                    Amount = x.Amount,
                    Description = x.Description,
                    IsActive = x.isActive
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

        //PUT: subscription/delete/{id}

        [HttpPut]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                object ret = new { isSuccess = false };
                var subscriptionIds = (from x in dbx.tbl_SubscriptionPlan select x.PlanID).ToList();

                if (subscriptionIds.Contains(id))
                {

                    SubscriptionRequest subs = new SubscriptionRequest();
                    ;
                    subs.IsActive = "false";

                    var existingPlan = dbx.tbl_SubscriptionPlan.Where(x => x.PlanID == id).FirstOrDefault();

                    existingPlan.isActive = subs.IsActive;

                    dbx.tbl_SubscriptionPlan.AddOrUpdate(existingPlan);
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

        //PUT:subscription/edit/{id}

        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult Update(int id, SubscriptionRequest value)
        {
            try
            {
                var planIds = (from x in dbx.tbl_SubscriptionPlan select x.PlanID).ToList();

                if (planIds.Contains(id))
                {

                    var existingPlan = dbx.tbl_SubscriptionPlan.Where(x => x.PlanID == id).FirstOrDefault();

                    existingPlan.PlanName = value.PlanName;
                    existingPlan.Amount = value.Amount;
                    existingPlan.Description = value.Description;

                    dbx.tbl_SubscriptionPlan.AddOrUpdate(existingPlan);
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

        //PUT: subscription/edit?{id}&{isActive}
        [HttpPut]
        [Route("edit")]
        public IHttpActionResult UpdateDonation(int id, string isActive)
        {
            try
            {
                var planIds = (from x in dbx.tbl_SubscriptionPlan select x.PlanID).ToList();

                if (planIds.Contains(id))
                {
                    SubscriptionRequest value = new SubscriptionRequest();

                    var existingPlan = dbx.tbl_SubscriptionPlan.Where(x => x.PlanID == id).FirstOrDefault();

                    existingPlan.isActive = value.IsActive = isActive;

                    dbx.tbl_SubscriptionPlan.AddOrUpdate(existingPlan);
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

        //PUT: subscription/assign

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

        //GET: subscription/validate

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
                    isSuccess = false,
                    code = 2,
                    status = "Subscription expired"
                };

                return Ok(ret);
            }
            return Json(ret);
        }
    }
}

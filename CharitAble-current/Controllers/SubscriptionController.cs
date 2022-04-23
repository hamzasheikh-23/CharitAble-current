using CharitAble_current.Models;
using CharitAble_current.Requests;
using System;
using System.Collections.Generic;
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


        //[HttpPut]
        //[Route("assign")]
        //public IHttpActionResult Assign(int userid)
        //{
        //    try
        //    {
        //        var userIds = (from x in dbx.tbl_NGOMaster select x.UserID).ToList();

        //        if (userIds.Contains(userid))
        //        {

        //            UserRequest user = new UserRequest();


        //            var existingDonation = dbx.tbl_Donations.Where(x => x.DonationID == id).FirstOrDefault();

        //            existingDonation.Status = donation.StatusId;
        //            existingDonation.isActive = donation.IsActive;

        //            dbx.tbl_Donations.AddOrUpdate(existingDonation);
        //            dbx.SaveChanges();

        //            return Json("record deleted successfully");
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Unable to process your request, check URL or user input\n" +
        //           "ErrorMessage: '" + ex.Message + "'");
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using CharitAble_current.Models;
using CharitAble_current.Requests;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("donation")]

    public class DonationController : ApiController
    {

        private charitable_dbEntities1 dbx = new charitable_dbEntities1();

        // POST donation/add
        [HttpPost]
        [Route("post")]
        public IHttpActionResult AddDonation(DonationRequest value)
        {
            object ret = new
            {
                code = "0",
                status = "Posting donation failed"
            };

            tbl_Donations donation = new tbl_Donations();
            donation.DonorID = value.DonorId;
            donation.DonationTitle = value.Title;
            donation.Quantity = value.Quantity;
            donation.Weight = value.Weight;
            donation.QuantityPerUnit = value.QuantityPerUnit;
            donation.ExpiryDate = value.ExpiryDate = DateTime.Now;
            donation.Description = value.Description;

            dbx.tbl_Donations.AddOrUpdate(donation);
            var result = dbx.SaveChanges();

            if (result != 0)
            {
                ret = new
                {
                    code = "1",
                    status = "Donation posted successfully"
                };
            }
            return Json(ret);
        }


    }

}
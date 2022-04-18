using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
using System.IO;
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

        // POST donation/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult AddDonation(DonationRequest value)
        {
            object ret = new
            {
                code = "0",
                status = "Posting donation failed"
            };


            var conditionId =
                (from x in dbx.tbl_DonationCondition
                 where x.Condition == value.Condition
                 select x.ConditionID).SingleOrDefault();

            tbl_Donations donation = new tbl_Donations();
            //donation.DonorID = value.DonorId;
            donation.DonorID = value.DonorId;
            donation.DonationTitle = value.Title;
            donation.Quantity = value.Quantity;
            donation.Weight = value.Weight;
            donation.QuantityPerUnit = 1;
            donation.Rating = value.Rating;
            donation.Condition = conditionId;
            donation.Category = value.CategoryId;
            donation.ExpiryDate = value.ExpiryDate = DateTime.Now;
            donation.Description = value.Description;




            //var firstImageBytes = Convert.FromBase64String(value.Image1);
            //var secondImageBytes = Convert.FromBase64String(value.Image2);
            //var thirdImageBytes = Convert.FromBase64String(value.Image3);

            // string filePath1 = Server.MapPath(@"F:\charitable uploaded images\" +



            if (!string.IsNullOrEmpty(value.Image1Name))
            {
                string imagePath1 = @"F:\charitable uploaded images\" + value.Image1Name;
                var cleanerBase1 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                donation.Image1 = imagePath1;
            }

            if (!string.IsNullOrEmpty(value.Image2Name))
            {
                string imagePath2 = @"F:\charitable uploaded images\" + value.Image2Name;
                var cleanerBase2 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                donation.Image2 = imagePath2;
            }

            if (!string.IsNullOrEmpty(value.Image3Name))
            {
                string imagePath3 = @"F:\charitable uploaded images\" + value.Image3Name;
                var cleanerBase3 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                File.WriteAllBytes(imagePath3, Convert.FromBase64String(cleanerBase3));
                donation.Image3 = imagePath3;
            }




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

        //GET donation/get/{id}

        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult GetDonation(int id)
        {
            var donations = dbx.tbl_Donations.Select(x =>
                new DonationRequest()
                {
                    DonationId = x.DonationID,
                    DonorId = x.DonorID,
                    Title = x.DonationTitle,
                    Quantity = x.Quantity,
                    Weight = x.Weight,
                    QuantityPerUnit = x.QuantityPerUnit,
                    ExpiryDate = x.ExpiryDate,
                    Description = x.Description
                }).Where(x => x.DonorId == id).ToList();

            return Json(donations);
        }

        //GET donation/get

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetDonation()
        {
            var donations = dbx.tbl_Donations.Select(x =>
                new DonationRequest()
                {
                    DonationId = x.DonationID,
                    DonorId = x.DonorID,
                    Title = x.DonationTitle,
                    Quantity = x.Quantity,
                    Weight = x.Weight,
                    QuantityPerUnit = x.QuantityPerUnit,
                    ExpiryDate = x.ExpiryDate,
                    Description = x.Description
                }).ToList();

            return Json(donations);
        }

        //GET donation/category/get

        [HttpGet]
        [Route("category/get")]
        public IHttpActionResult GetDonationCategory()
        {
            var donations = dbx.tbl_DonationCategory.Select(x =>
                new DonationCategoryRequest()
                {
                    CategoryId = x.CategoryID,
                    DonationCategory = x.DonationCategory
                }).ToList();

            return Json(donations);
        }

    }
}
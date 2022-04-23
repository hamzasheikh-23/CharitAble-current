﻿using System;
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
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting donation failed"
                };


                var conditionId = (from x in dbx.tbl_DonationCondition
                                   where x.Condition == value.Condition
                                   select x.ConditionID).SingleOrDefault();

                var statusId = (from x in dbx.tbl_DonationStatus
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                var isActive = "true";

                tbl_Donations donation = new tbl_Donations();
                donation.DonorID = value.DonorId;
                donation.DonationTitle = value.Title;
                donation.Quantity = value.Quantity;
                donation.Weight = value.Weight;
                donation.QuantityPerUnit = value.QuantityPerUnit;
                donation.Rating = value.Rating;
                donation.Condition = value.ConditionId = conditionId;
                donation.Category = value.CategoryId;
                donation.PostedDateTime = DateTime.Now;
                donation.Status = value.StatusId = statusId;
                donation.isActive = value.IsActive = isActive;
                donation.ExpiryDate = value.ExpiryDate;
                donation.Description = value.Description;


                //var firstImageBytes = Convert.FromBase64String(value.Image1);
                //var secondImageBytes = Convert.FromBase64String(value.Image2);
                //var thirdImageBytes = Convert.FromBase64String(value.Image3);

                // string filePath1 = Server.MapPath(@"F:\charitable uploaded images\" +



                if (!string.IsNullOrEmpty(value.Image1Name))
                {
                    Guid obj = Guid.NewGuid();
                    string imagePath1 = @"F:\charitable uploaded images\" + DateTime.Now.ToString("hh:mm:ss.fff MM/dd/yyyy") + obj.ToString();
                    var cleanerBase1 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                    donation.Image1 = imagePath1;
                }

                if (!string.IsNullOrEmpty(value.Image2Name))
                {
                    Guid obj = Guid.NewGuid();
                    string imagePath2 = @"F:\charitable uploaded images\" + DateTime.Now.ToString("hh:mm:ss.fff MM/dd/yyyy") + obj.ToString();
                    var cleanerBase2 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                    donation.Image2 = imagePath2;
                }

                if (!string.IsNullOrEmpty(value.Image3Name))
                {
                    Guid obj = Guid.NewGuid();
                    string imagePath3 = @"F:\charitable uploaded images\" + DateTime.Now.ToString("hh:mm:ss.fff MM/dd/yyyy") + obj.ToString();
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
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }


        //GET donation/get

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetDonation()
        {
            try
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
                    PostedDate = x.PostedDateTime,
                    Status = (from y in dbx.tbl_DonationStatus
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    LocationCo = x.LocationCoordinates,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).ToList();

                if (donations.Count > 0)
                {
                    return Json(donations);
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


        //GET donation/get?{donorId}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetDonation(int donorId)
        {
            try
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
                    PostedDate = x.PostedDateTime,
                    Status = (from y in dbx.tbl_DonationStatus
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    LocationCo = x.LocationCoordinates,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId).ToList();

                return Json(donations);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }

        //GET donation/get?{donorId}&&{status}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetDonation(int donorId, string status)
        {
            try
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
                    PostedDate = x.PostedDateTime,
                    Status = (from y in dbx.tbl_DonationStatus
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    LocationCo = x.LocationCoordinates,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId && x.Status == status).ToList();

                return Json(donations);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }


        //GET donation/get?{donorId}&&{isActive}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetD(int donorId, string isActive)
        {
            try
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
                    PostedDate = x.PostedDateTime,
                    Status = (from y in dbx.tbl_DonationStatus
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    LocationCo = x.LocationCoordinates,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId && x.IsActive == isActive).ToList();

                return Json(donations);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }


        //GET donation/get?{donorId}&&{status}&&{isActive}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetDonation(int donorId, string status, string isActive)
        {
            try
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
                    PostedDate = x.PostedDateTime,
                    Status = (from y in dbx.tbl_DonationStatus
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    LocationCo = x.LocationCoordinates,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId && x.Status == status && x.IsActive == isActive).ToList();

                return Json(donations);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                    "ErrorMessage: '" + ex.Message + "'");
            }
        }


        //GET donation/category/get

        [HttpGet]
        [Route("category/get")]
        public IHttpActionResult GetDonationCategory()
        {
            try
            {
                var donations = dbx.tbl_DonationCategory.Select(x =>
                new DonationCategoryRequest()
                {
                    CategoryId = x.CategoryID,
                    DonationCategory = x.DonationCategory
                }).ToList();

                return Json(donations);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request, check URL or user input\n" +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }

        //PUT: donation/delete/{id}

        [HttpPut]
        [Route("delete/{id}")]
        public IHttpActionResult DeleteDonation(int id)
        {
            try
            {
                var donationIds = (from x in dbx.tbl_Donations select x.DonationID).ToList();

                if (donationIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_DonationStatus
                                    where x.Status == "Deleted" || x.Status == "deleted"
                                    select x.StatusID).SingleOrDefault();

                    DonationRequest donation = new DonationRequest();

                    donation.StatusId = statusId;
                    donation.IsActive = "false";

                    var existingDonation = dbx.tbl_Donations.Where(x => x.DonationID == id).FirstOrDefault();

                    existingDonation.Status = donation.StatusId;
                    existingDonation.isActive = donation.IsActive;

                    dbx.tbl_Donations.AddOrUpdate(existingDonation);
                    dbx.SaveChanges();

                    return Json("record deleted successfully");
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


        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult UpdateDonation(int id, DonationRequest value)
        {
            try
            {
                var donationIds = (from x in dbx.tbl_Donations select x.DonationID).ToList();

                if (donationIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_DonationStatus
                                    where x.Status == "Pending" || x.Status == "pending"
                                    select x.StatusID).SingleOrDefault();

                    var conditionId = (from x in dbx.tbl_DonationCondition
                                       where x.Condition == value.Condition
                                       select x.ConditionID).SingleOrDefault();

                    var categoryId = (from x in dbx.tbl_DonationCategory
                                      where x.DonationCategory == value.Category
                                      select x.CategoryID).SingleOrDefault();

                    var existingDonation = dbx.tbl_Donations.Where(x => x.DonationID == id).FirstOrDefault();

                    existingDonation.DonorID = value.DonorId;
                    existingDonation.DonationTitle = value.Title;
                    existingDonation.Quantity = value.Quantity;
                    existingDonation.Weight = value.Weight;
                    existingDonation.QuantityPerUnit = value.QuantityPerUnit;
                    existingDonation.ExpiryDate = value.ExpiryDate;
                    existingDonation.Status = value.StatusId = statusId;
                    existingDonation.Description = value.Description;
                    existingDonation.Rating = value.Rating;
                    existingDonation.Condition = value.ConditionId = conditionId;
                    existingDonation.Category = value.CategoryId = categoryId;
                    existingDonation.LocationCoordinates = value.LocationCo;

                    if (!string.IsNullOrWhiteSpace(value.Image1Name))
                    {
                        Guid obj = Guid.NewGuid();
                        string imagePath1 = @"F:\charitable uploaded images\" + DateTime.Now.ToString("hh:mm:ss.fff MM/dd/yyyy") + obj.ToString();
                        var cleanerBase1 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                        existingDonation.Image1 = imagePath1;
                    }

                    if (!string.IsNullOrWhiteSpace(value.Image2Name))
                    {
                        Guid obj = Guid.NewGuid();
                        string imagePath2 = @"F:\charitable uploaded images\" + DateTime.Now.ToString("hh:mm:ss.fff MM/dd/yyyy") + obj.ToString();
                        var cleanerBase2 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                        existingDonation.Image2 = imagePath2;
                    }

                    if (!string.IsNullOrWhiteSpace(value.Image3Name))
                    {
                        Guid obj = Guid.NewGuid();
                        string imagePath3 = @"F:\charitable uploaded images\" + DateTime.Now.ToString("hh:mm:ss.fff MM/dd/yyyy") + obj.ToString();
                        var cleanerBase3 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath3, Convert.FromBase64String(cleanerBase3));
                        existingDonation.Image3 = imagePath3;
                    }

                    dbx.tbl_Donations.AddOrUpdate(existingDonation);
                    dbx.SaveChanges();

                    return Json("record deleted successfully");
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
    }
}
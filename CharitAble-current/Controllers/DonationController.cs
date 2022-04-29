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
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting donation failed"
                };


                var statusId = (from x in dbx.tbl_Status
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                var isActive = "true";

                tbl_Donations donation = new tbl_Donations();
                donation.DonorID = (int?)value.DonorId;
                donation.DonationTitle = value.Title;
                donation.Quantity = value.Quantity;
                donation.Weight = value.Weight;
                donation.QuantityPerUnit = value.QuantityPerUnit;
                donation.Rating = value.Rating;
                donation.Condition = value.ConditionId;
                donation.Category = value.Category;
                donation.PostedDateTime = DateTime.Now;
                donation.Status = value.StatusId = statusId;
                donation.isActive = value.IsActive = isActive;
                donation.Address = value.Address;
                donation.ExpiryDate = value.ExpiryDate;
                donation.Description = value.Description;


                //var firstImageBytes = Convert.FromBase64String(value.Image1);
                //var secondImageBytes = Convert.FromBase64String(value.Image2);
                //var thirdImageBytes = Convert.FromBase64String(value.Image3);

                // string filePath1 = Server.MapPath(@"F:\charitable uploaded images\" +



                if (!string.IsNullOrEmpty(value.Image1Name))
                {
                    string imagePath1 = @"D:\fyp-frontend\src\serverImages\donations" + value.Image1Name;
                    FileInfo fi = new FileInfo(imagePath1);
                    Guid obj = Guid.NewGuid();
                    imagePath1 = @"D:\fyp-frontend\src\serverImages\donations" + obj.ToString() + fi.Extension;
                    var cleanerBase1 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                    donation.Image1 = imagePath1;
                }

                if (!string.IsNullOrEmpty(value.Image2Name))
                {
                    string imagePath2 = @"D:\fyp-frontend\src\serverImages\donations" + value.Image2Name;
                    FileInfo fi = new FileInfo(imagePath2);
                    Guid obj = Guid.NewGuid();
                    imagePath2 = @"D:\fyp-frontend\src\serverImages\donations" + obj.ToString() + fi.Extension;
                    var cleanerBase2 = value.Image2base64.Substring(value.Image2base64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                    donation.Image2 = imagePath2;
                }

                if (!string.IsNullOrEmpty(value.Image3Name))
                {
                    string imagePath3 = @"D:\fyp-frontend\src\serverImages\donations" + value.Image3Name;
                    FileInfo fi = new FileInfo(imagePath3);
                    Guid obj = Guid.NewGuid();
                    imagePath3 = @"D:\fyp-frontend\src\serverImages\donations" + obj.ToString() + fi.Extension;
                    var cleanerBase3 = value.Image3base64.Substring(value.Image3base64.LastIndexOf(',') + 1);
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
                    StatusId = x.Status,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    ConditionId = x.Condition,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = x.Category,
                    category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    Address = x.Address,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).ToList();

                if (donations.Count > 0)
                {
                    return Ok(donations);
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
                    StatusId = x.Status,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    ConditionId = x.Condition,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = x.Category,
                    category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    Address = x.Address,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId).ToList();

                foreach (DonationRequest item in donations)
                {
                    if (!string.IsNullOrWhiteSpace(item.Image1))
                    {
                        string imagePath1 = item.Image1;
                        FileInfo fi = new FileInfo(imagePath1);
                        item.Image1Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image2))
                    {
                        string imagePath2 = item.Image2;
                        FileInfo fi = new FileInfo(imagePath2);
                        item.Image2Name = fi.Name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Image3))
                    {
                        string imagePath3 = item.Image3;
                        FileInfo fi = new FileInfo(imagePath3);
                        item.Image3Name = fi.Name;
                    }

                    //item.Image1 = string.IsNullOrWhiteSpace(item.Image1) ? item.Image1 : Convert.ToBase64String(File.ReadAllBytes(item.Image1));
                    //item.Image2 = string.IsNullOrWhiteSpace(item.Image2) ? item.Image2 : Convert.ToBase64String(File.ReadAllBytes(item.Image2));
                    //item.Image3 = string.IsNullOrWhiteSpace(item.Image3) ? item.Image3 : Convert.ToBase64String(File.ReadAllBytes(item.Image3));
                }
                return Ok(donations);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to process your request. " +
                   "ErrorMessage: '" + ex.Message + "'");
            }
        }

        //GET donation/get?{status}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetDonation(string status)
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
                    StatusId = x.Status,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    ConditionId = x.Condition,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = x.Category,
                    category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    Address = x.Address,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.Status == status).ToList();

                return Ok(donations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex + ": '" + ex.Message + "'");
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
                    StatusId = x.Status,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    ConditionId = x.Condition,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = x.Category,
                    category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    Address = x.Address,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId && x.Status == status).ToList();

                return Ok(donations);
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
                    StatusId = x.Status,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    ConditionId = x.Condition,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = x.Category,
                    category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    Address = x.Address,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId && x.IsActive == isActive).ToList();

                return Ok(donations);
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
                    StatusId = x.Status,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.Status
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    Description = x.Description,
                    Rating = x.Rating,
                    ConditionId = x.Condition,
                    Condition = (from y in dbx.tbl_DonationCondition
                                 where y.ConditionID == x.Condition
                                 select y.Condition).FirstOrDefault().Trim(),
                    Category = x.Category,
                    category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.Category
                                select y.DonationCategory).FirstOrDefault().Trim(),
                    Address = x.Address,
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                }).Where(x => x.DonorId == donorId && x.Status == status && x.IsActive == isActive).ToList();

                return Ok(donations);
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

                return Ok(donations);
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
                    var statusId = (from x in dbx.tbl_Status
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

                    return Ok("record deleted successfully");
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
                    //var statusId = (from x in dbx.tbl_Status
                    //                where x.Status == "Pending" || x.Status == "pending"
                    //                select x.StatusID).SingleOrDefault();

                    //var conditionId = (from x in dbx.tbl_DonationCondition
                    //                   where x.Condition == value.Condition
                    //                   select x.ConditionID).SingleOrDefault();

                    //var categoryId = (from x in dbx.tbl_DonationCategory
                    //                  where x.DonationCategory == value.Category
                    //                  select x.CategoryID).SingleOrDefault();

                    var existingDonation = dbx.tbl_Donations.Where(x => x.DonationID == id).FirstOrDefault();

                    existingDonation.DonorID = value.DonorId;
                    existingDonation.DonationTitle = value.Title;
                    existingDonation.Quantity = value.Quantity;
                    existingDonation.Weight = value.Weight;
                    existingDonation.QuantityPerUnit = value.QuantityPerUnit;
                    existingDonation.ExpiryDate = value.ExpiryDate;
                    // existingDonation.Status = value.StatusId;
                    existingDonation.Description = value.Description;
                    existingDonation.Rating = value.Rating;
                    existingDonation.Condition = value.ConditionId;
                    existingDonation.Category = value.Category;
                    existingDonation.Address = value.Address;

                    if (!string.IsNullOrEmpty(value.Image1Name))
                    {
                        string imagePath1 = @"D:\fyp-frontend\src\serverImages\donations" + value.Image1Name;
                        FileInfo fi = new FileInfo(imagePath1);
                        Guid obj = Guid.NewGuid();
                        imagePath1 = @"D:\fyp-frontend\src\serverImages\donations" + obj.ToString() + fi.Extension;
                        var cleanerBase1 = value.Image1base64.Substring(value.Image1base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath1, Convert.FromBase64String(cleanerBase1));
                        existingDonation.Image1 = imagePath1;
                    }

                    if (!string.IsNullOrEmpty(value.Image2Name))
                    {
                        string imagePath2 = @"D:\fyp-frontend\src\serverImages\donations" + value.Image2Name;
                        FileInfo fi = new FileInfo(imagePath2);
                        Guid obj = Guid.NewGuid();
                        imagePath2 = @"D:\fyp-frontend\src\serverImages\donations" + obj.ToString() + fi.Extension;
                        var cleanerBase2 = value.Image2base64.Substring(value.Image2base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath2, Convert.FromBase64String(cleanerBase2));
                        existingDonation.Image2 = imagePath2;
                    }

                    if (!string.IsNullOrEmpty(value.Image3Name))
                    {
                        string imagePath3 = @"D:\fyp-frontend\src\serverImages\donations" + value.Image3Name;
                        FileInfo fi = new FileInfo(imagePath3);
                        Guid obj = Guid.NewGuid();
                        imagePath3 = @"D:\fyp-frontend\src\serverImages\donations" + obj.ToString() + fi.Extension;
                        var cleanerBase3 = value.Image3base64.Substring(value.Image3base64.LastIndexOf(',') + 1);
                        File.WriteAllBytes(imagePath3, Convert.FromBase64String(cleanerBase3));
                        existingDonation.Image3 = imagePath3;
                    }

                    dbx.tbl_Donations.AddOrUpdate(existingDonation);
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

        [HttpPut]
        [Route("edit")]
        public IHttpActionResult UpdateDonation(int id, string status)
        {
            try
            {
                var donationIds = (from x in dbx.tbl_Donations select x.DonationID).ToList();

                if (donationIds.Contains(id))
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

                    DonationRequest value = new DonationRequest();

                    var existingDonation = dbx.tbl_Donations.Where(x => x.DonationID == id).FirstOrDefault();

                    existingDonation.Status = value.StatusId = statusId;

                    dbx.tbl_Donations.AddOrUpdate(existingDonation);
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
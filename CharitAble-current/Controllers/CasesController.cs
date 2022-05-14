using CharitAble_current.Requests;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using CharitAble_current.Models;
using System.IO;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("case")]
    public class CasesController : ApiController
    {
        charitable_dbEntities2 dbx = new charitable_dbEntities2();

        //POST: case/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(CaseRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting case failed"
                };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                var isActive = "true";

                var categoryId = (from x in dbx.tbl_DonationCategory
                                  where x.DonationCategory == value.DonationCategory
                                  select x.CategoryID).SingleOrDefault();

                var unitId = (from x in dbx.tbl_Units
                              where x.Unit == value.Unit
                              select x.UnitID).SingleOrDefault();

                tbl_Cases cases = new tbl_Cases();
                cases.NGO_ID = value.NGOId;
                cases.CaseTitle = value.CaseTitle;
                cases.PostedDate = value.PostedDate = DateTime.Now;
                cases.Description = value.Description;
                cases.CategoryID = value.CategoryId = 1;
                cases.isActive = value.IsActive = isActive;
                cases.Quantity = value.Quantity;
                cases.StatusID = value.StatusId = statusId;
                cases.UnitID = value.UnitId = 2;

                if (!string.IsNullOrEmpty(value.ImageBase64))
                {
                    //string imagePath = @"D:\fyp-frontend\src\serverImages\cases\" + value.ImageName;
                    //FileInfo fi = new FileInfo(imagePath);
                    //Guid obj = Guid.NewGuid();
                    //imagePath = @"D:\fyp-frontend\src\serverImages\cases\" + obj.ToString() + fi.Extension;
                    var cleanerBase = value.ImageBase64.Substring(value.ImageBase64.LastIndexOf(',') + 1);
                    //File.WriteAllBytes(imagePath, Convert.FromBase64String(cleanerBase));
                    cases.CoverImage = cleanerBase;
                }


                dbx.tbl_Cases.AddOrUpdate(cases);
                var result = dbx.SaveChanges();


                if (result != 0)
                {
                    ret = new
                    {
                        code = "1",
                        status = "Case posted successfully"
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
                object ret = new { noData = true, status = "unsuccesfull request" };

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //GET: case/get?{id}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase(int ngoId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).Where(x => x.NGOId == ngoId).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //GET: case/get?{id}&&{isActive}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase(int ngoId, string isActive)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).Where(x => x.NGOId == ngoId && x.IsActive == isActive).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //GET: case/get?{id}&&{status}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetC(int ngoId, string status)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).Where(x => x.NGOId == ngoId && x.Status == status).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //GET: case/get?{id}&&{status}&&{isActive}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase(int ngoId, string status, string isActive)
        {
            try
            {
                object ret = new { status = "unsuccesfull request", noData = true };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).FirstOrDefault();

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).Where(x => x.NGOId == ngoId && x.StatusId == statusId && x.IsActive == isActive).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }


        //GET: case/get?{id}&&{status}&&{isActive}&&{category}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase(int ngoId, string status, string isActive, string category)
        {
            try
            {
                object ret = new { status = "unsuccesfull request", noData = true };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).FirstOrDefault();

                var categoryId = (from x in dbx.tbl_DonationCategory
                                  where x.DonationCategory == category
                                  select x.CategoryID).FirstOrDefault();

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).Where(x => x.NGOId == ngoId && x.StatusId == statusId && x.IsActive == isActive && x.CategoryId == categoryId).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //GET: case/get?{category}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase(string category)
        {
            try
            {
                object ret = new { status = "unsuccesfull request", noData = true };


                var categoryId = (from x in dbx.tbl_DonationCategory
                                  where x.DonationCategory == category
                                  select x.CategoryID).FirstOrDefault();

                var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault(),
                    CaseTitle = x.CaseTitle,
                    CategoryId = x.CategoryID,
                    DonationCategory = (from y in dbx.tbl_DonationCategory
                                        where y.CategoryID == x.CategoryID
                                        select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault(),
                    IsActive = x.isActive,
                    UnitId = x.UnitID,
                    Unit = (from y in dbx.tbl_Units
                            where y.UnitID == x.UnitID
                            select y.Unit).FirstOrDefault(),

                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description,
                    CoverImage = x.CoverImage
                }).Where(x => x.CategoryId == categoryId).ToList();

                //foreach (CaseRequest item in cases)
                //{
                //    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                //    {
                //        string imagePath = item.CoverImage;
                //        FileInfo fi = new FileInfo(imagePath);
                //        item.ImageName = fi.Name;
                //    }
                //}

                if (cases.Any())
                {
                    ret = new { cases, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //PUT: case/delete/{id}

        [HttpPut]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                object ret = new { isSuccess = false };
                var caseIds = (from x in dbx.tbl_Cases select x.CaseID).ToList();

                if (caseIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == "Deleted" || x.Status == "deleted"
                                    select x.StatusID).SingleOrDefault();

                    CaseRequest cases = new CaseRequest();

                    cases.StatusId = statusId;
                    cases.IsActive = "false";

                    var existingCase = dbx.tbl_Cases.Where(x => x.CaseID == id).FirstOrDefault();

                    existingCase.StatusID = cases.StatusId;
                    existingCase.isActive = cases.IsActive;

                    dbx.tbl_Cases.AddOrUpdate(existingCase);
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

        //PUT:case/edit/{id}

        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult Update(int id, CaseRequest value)
        {
            try
            {
                var caseIds = (from x in dbx.tbl_Cases select x.CaseID).ToList();

                if (caseIds.Contains(id))
                {

                    var existingCase = dbx.tbl_Cases.Where(x => x.CaseID == id).FirstOrDefault();

                    existingCase.NGO_ID = value.NGOId;
                    existingCase.CaseTitle = value.CaseTitle;
                    existingCase.Description = value.Description;
                    existingCase.CategoryID = (from x in dbx.tbl_DonationCategory
                                               where x.DonationCategory == value.DonationCategory
                                               select x.CategoryID).FirstOrDefault();
                    existingCase.isActive = value.IsActive;
                    existingCase.Quantity = value.Quantity;
                    existingCase.StatusID = (from x in dbx.tbl_Status
                                             where x.Status == "Pending" || x.Status == "pending"
                                             select x.StatusID).FirstOrDefault();
                    existingCase.UnitID = (from x in dbx.tbl_Units
                                           where x.Unit == value.Unit
                                           select x.UnitID).FirstOrDefault();

                    if (!string.IsNullOrEmpty(value.ImageBase64))
                    {
                        //string imagePath = @"D:\fyp-frontend\src\serverImages\cases\" + value.ImageName;
                        //FileInfo fi = new FileInfo(imagePath);
                        //Guid obj = Guid.NewGuid();
                        //imagePath = @"D:\fyp-frontend\src\serverImages\cases\" + obj.ToString() + fi.Extension;
                        var cleanerBase = value.ImageBase64.Substring(value.ImageBase64.LastIndexOf(',') + 1);
                        //File.WriteAllBytes(imagePath, Convert.FromBase64String(cleanerBase));
                        existingCase.CoverImage = cleanerBase;
                    }

                    dbx.tbl_Cases.AddOrUpdate(existingCase);
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

        //PUT: case/edit?{id}&{status}
        [HttpPut]
        [Route("edit")]
        public IHttpActionResult UpdateDonation(int id, string status)
        {
            try
            {
                var caseIds = (from x in dbx.tbl_Cases select x.CaseID).ToList();

                if (caseIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == status
                                    select x.StatusID).SingleOrDefault();

                    CaseRequest value = new CaseRequest();

                    var existingCase = dbx.tbl_Cases.Where(x => x.CaseID == id).FirstOrDefault();

                    existingCase.StatusID = value.StatusId = statusId;

                    dbx.tbl_Cases.AddOrUpdate(existingCase);
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

        //PUT: case/edit?{id}&{status}&{isActive}
        [HttpPut]
        [Route("edit")]
        public IHttpActionResult UpdateDonation(int id, string status, string isActive)
        {
            try
            {
                var caseIds = (from x in dbx.tbl_Cases select x.CaseID).ToList();

                if (caseIds.Contains(id))
                {
                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == status
                                    select x.StatusID).SingleOrDefault();

                    CaseRequest value = new CaseRequest();

                    var existingCase = dbx.tbl_Cases.Where(x => x.CaseID == id).FirstOrDefault();

                    existingCase.StatusID = value.StatusId = statusId;
                    existingCase.isActive = value.IsActive = isActive;

                    dbx.tbl_Cases.AddOrUpdate(existingCase);
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
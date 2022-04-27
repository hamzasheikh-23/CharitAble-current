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
        charitable_dbEntities1 dbx = new charitable_dbEntities1();

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

                var statusId = (from x in dbx.tbl_DonationStatus
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                var isActive = "true";

                var categoryId = (from x in dbx.tbl_DonationCategory
                                  where x.DonationCategory == value.Category
                                  select x.CategoryID).SingleOrDefault();

                var unitId = (from x in dbx.tbl_Units
                              where x.Unit == value.Unit
                              select x.UnitID).SingleOrDefault();

                tbl_Cases cases = new tbl_Cases();
                cases.NGO_ID = value.NGOId;
                cases.CaseTitle = value.CaseTitle;
                cases.PostedDate = value.PostedDate = DateTime.Now;
                cases.Description = value.Description;
                cases.CategoryID = value.CategoryId = categoryId;
                cases.isActive = value.IsActive = isActive;
                cases.Quantity = value.Quantity;
                cases.StatusID = value.StatusId = statusId;
                cases.UnitID = value.UnitId = unitId;

                if (!string.IsNullOrEmpty(value.ImageName))
                {
                    string imagePath = @"D:\fyp-frontend\src\serverImages\" + value.ImageName;
                    FileInfo fi = new FileInfo(imagePath);
                    Guid obj = Guid.NewGuid();
                    imagePath = @"D:\fyp-frontend\src\serverImages\" + obj.ToString() + fi.Extension;
                    var cleanerBase = value.ImageBase64.Substring(value.ImageBase64.LastIndexOf(',') + 1);
                    File.WriteAllBytes(imagePath, Convert.FromBase64String(cleanerBase));
                    value.CoverImage = imagePath;
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
                return BadRequest(ex.Message);
            }
        }

        // GET: case/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

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
                    Category = (from y in dbx.tbl_DonationCategory
                                where y.CategoryID == x.CategoryID
                                select y.DonationCategory).FirstOrDefault(),
                    Quantity = x.Quantity,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_DonationStatus
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
                }).ToList();

                foreach (CaseRequest item in cases)
                {
                    if (!string.IsNullOrWhiteSpace(item.CoverImage))
                    {
                        string imagePath = item.CoverImage;
                        FileInfo fi = new FileInfo(imagePath);
                        item.ImageName = fi.Name;
                    }
                }
                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //GET: case/get/{id}

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase(int id)
        {
            var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = (int)x.NGO_ID,
                    CaseTitle = x.CaseTitle,
                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description
                }).Where(x => x.NGOId == id).ToList();

            return Json(cases);
        }
    }
}
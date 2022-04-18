using CharitAble_current.Requests;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using CharitAble_current.Models;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("case")]
    public class CasesController : ApiController
    {
        charitable_dbEntities1 dbx = new charitable_dbEntities1();

        //POST: case/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult AddCase(CaseRequest value)
        {
            object ret = new
            {
                code = "0",
                status = "Posting case failed"
            };

            tbl_Cases cases = new tbl_Cases();
            cases.CaseID = value.CaseId;
            cases.NGO_ID = value.NGOId;
            cases.CaseTitle = value.CaseTitle;
            cases.PostedDate = value.PostedDate = DateTime.Now;
            cases.Description = value.Description;

            //cases.Picture = value.Picture;

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
            return Json(ret);
        }

        // GET: case/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetCase()
        {
            var cases = dbx.tbl_Cases.Select(x =>
                new CaseRequest()
                {
                    CaseId = x.CaseID,
                    NGOId = (int)x.NGO_ID,
                    CaseTitle = x.CaseTitle,
                    PostedDate = (DateTime)x.PostedDate,
                    Description = x.Description
                }).ToList();

            return Json(cases);
        }

        //GET: case/get/{id}

        [HttpGet]
        [Route("get/{id}")]
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
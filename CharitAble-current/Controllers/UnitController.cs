using CharitAble_current.Models;
using CharitAble_current.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("unit")]
    public class UnitController : ApiController
    {
        charitable_dbEntities1 dbx = new charitable_dbEntities1();

        // GET: unit/get

        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var unit = dbx.tbl_Units.Select(x =>
                new UnitRequest()
                {
                    UnitId = x.UnitID,
                    Unit = x.Unit
                }).ToList();


                if (unit.Any())
                {
                    return Ok(unit);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }
    }
}
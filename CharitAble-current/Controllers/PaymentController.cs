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
    [RoutePrefix("paymentInfo")]
    public class PaymentController : ApiController
    {
        charitable_dbEntities1 dbx = new charitable_dbEntities1();

        //POST: paymentInfo/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(PaymentInfoRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting payment info failed"
                };

                tbl_PaymentInfo info = new tbl_PaymentInfo()
                {
                    NGO_ID = value.NgoId,
                    CardNumber = value.CardNumber,
                    CurrentExpiryMonth = value.ExpiryMonth,
                    CurretnExpiryYear = value.ExpiryYear,
                    CardholderName = value.CardholderName,
                    CVV = value.CVV
                };


                dbx.tbl_PaymentInfo.AddOrUpdate(info);
                var result = dbx.SaveChanges();


                if (result != 0)
                {
                    var lastId = (from x in dbx.tbl_PaymentInfo
                                  orderby x.PaymentInfoID descending
                                  select x.PaymentInfoID).FirstOrDefault();
                    ret = new
                    {
                        lastId,
                        code = "1",
                        status = "Payment info posted successfully"
                    };
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: paymentInfo/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var info = dbx.tbl_PaymentInfo.Select(x =>
                new PaymentInfoRequest()
                {
                    PaymentInfoId = x.PaymentInfoID,
                    NgoId = x.NGO_ID,
                    CardholderName = x.CardholderName,
                    CardNumber = x.CardNumber,
                    ExpiryMonth = x.CurrentExpiryMonth,
                    ExpiryYear = x.CurretnExpiryYear,
                    CVV = x.CVV,
                }).ToList();


                if (info.Any())
                {
                    ret = new { info, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: paymentInfo/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int ngoId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var info = dbx.tbl_PaymentInfo.Select(x =>
                new PaymentInfoRequest()
                {
                    PaymentInfoId = x.PaymentInfoID,
                    NgoId = x.NGO_ID,
                    CardholderName = x.CardholderName,
                    CardNumber = x.CardNumber,
                    ExpiryMonth = x.CurrentExpiryMonth,
                    ExpiryYear = x.CurretnExpiryYear,
                    CVV = x.CVV,
                }).Where(x => x.NgoId == ngoId).ToList();


                if (info.Any())
                {
                    ret = new { info, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }
    }
}
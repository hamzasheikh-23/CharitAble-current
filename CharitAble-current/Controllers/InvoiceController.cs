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
    [RoutePrefix("invoice")]
    public class InvoiceController : ApiController
    {
        charitable_dbEntities1 dbx = new charitable_dbEntities1();

        //POST: invoice/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(InvoiceRequest value)
        {
            try
            {
                object ret = new
                {
                    isSuccess = false,
                    code = "0",
                    status = "Posting invoice failed"
                };

                tbl_Invoices invoice = new tbl_Invoices()
                {
                    OrderID = value.OrderId,
                    Date = value.Date = DateTime.Now.Date,
                };


                dbx.tbl_Invoices.AddOrUpdate(invoice);
                var result = dbx.SaveChanges();


                if (result != 0)
                {
                    ret = new
                    {
                        isSuccess = true,
                        code = "1",
                        status = "Invoice posted successfully"
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

                var invoices = dbx.tbl_Invoices.Select(x =>
                new InvoiceRequest()
                {
                    InvoiceId = x.InvoiceID,
                    OrderId = x.OrderID,
                    Date = x.Date,
                    Amount = (from o in dbx.tbl_Orders
                              where o.OrderID == x.OrderID
                              select o.Amount).FirstOrDefault()
                }).ToList();


                if (invoices.Any())
                {
                    ret = new { invoices, noData = false };
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

                var invoices = (from i in dbx.tbl_Invoices
                                join o in dbx.tbl_Orders on i.OrderID equals o.OrderID
                                join c in dbx.tbl_Cases on o.CaseID equals c.CaseID
                                where c.NGO_ID == ngoId
                                select new InvoiceRequest()
                                {
                                    InvoiceId = i.InvoiceID,
                                    OrderId = i.OrderID,
                                    Date = i.Date,
                                    Amount = (from o in dbx.tbl_Orders
                                              where o.OrderID == i.OrderID
                                              select o.Amount).FirstOrDefault()
                                }).ToList();

                if (invoices.Any())
                {
                    ret = new { invoices, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetO(int orderId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var invoices = (from i in dbx.tbl_Invoices
                                join o in dbx.tbl_Orders on i.OrderID equals o.OrderID
                                join p in dbx.tbl_PaymentInfo on o.PaymentID equals p.PaymentInfoID
                                where o.OrderID == orderId
                                select new InvoiceRequest()
                                {
                                    InvoiceId = i.InvoiceID,
                                    OrderId = i.OrderID,
                                    Date = i.Date,
                                    Amount = o.Amount,
                                    CardNumber = p.CardNumber,
                                    CardholderName = p.CardholderName
                                }).FirstOrDefault();

                if (invoices != null)
                {
                    ret = new { invoices, noData = false };
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
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
    [RoutePrefix("order")]
    public class OrdersController : ApiController
    {
        charitable_dbEntities1 dbx = new charitable_dbEntities1();
        // POST: order/post
        [HttpPost]
        [Route("post")]
        public IHttpActionResult Post(OrderRequest value)
        {
            try
            {
                object ret = new
                {
                    code = "0",
                    status = "Posting order failed"
                };

                var statusId = (from x in dbx.tbl_Status
                                where x.Status == "Pending" || x.Status == "pending"
                                select x.StatusID).SingleOrDefault();

                //var isActive = "true";

                tbl_Orders order = new tbl_Orders();
                order.NGO_ID = value.NGOId;
                order.CaseID = value.CaseId;
                order.ReplyID = value.ReplyId;
                order.DeliveryAddress = value.DeliveryAddress;
                order.OrderDateTime = value.OrderDateTime = DateTime.Now;
                order.StatusID = value.StatusId = statusId;


                dbx.tbl_Orders.AddOrUpdate(order);
                var result = dbx.SaveChanges();

                if (result != 0)
                {
                    ret = new
                    {
                        code = "1",
                        status = "order posted successfully"
                    };
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: order/get
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var order = dbx.tbl_Orders.Select(x =>
                new OrderRequest()
                {
                    OrderId = x.OrderID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    ReplyId = x.ReplyID,
                    DeliveryAddress = x.DeliveryAddress,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    OrderDateTime = x.OrderDateTime
                }).ToList();


                if (order.Any())
                {
                    return Ok(order);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: order/get?{orderId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int orderId)
        {
            try
            {
                object ret = new { code = 0, status = "unsuccesfull request" };

                var order = dbx.tbl_Orders.Select(x =>
                new OrderRequest()
                {
                    OrderId = x.OrderID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    ReplyId = x.ReplyID,
                    DeliveryAddress = x.DeliveryAddress,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    OrderDateTime = x.OrderDateTime
                }).Where(x => x.OrderId == orderId).ToList();


                if (order.Any())
                {
                    return Ok(order);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: order/get?{orderId}&&{status}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get(int orderId, string status)
        {
            try
            {
                var statusId = (from x in dbx.tbl_Status
                                where x.Status == status
                                select x.StatusID).SingleOrDefault();

                object ret = new { code = 0, status = "unsuccesfull request" };

                var order = dbx.tbl_Orders.Select(x =>
                new OrderRequest()
                {
                    OrderId = x.OrderID,
                    NGOId = x.NGO_ID,
                    NGOName = (from n in dbx.tbl_NGOMaster
                               join u in dbx.tbl_Users on n.UserID equals u.UserID
                               where n.NGO_ID == x.NGO_ID
                               select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                    CaseId = x.CaseID,
                    ReplyId = x.ReplyID,
                    DeliveryAddress = x.DeliveryAddress,
                    StatusId = x.StatusID,
                    Status = (from y in dbx.tbl_Status
                              where y.StatusID == x.StatusID
                              select y.Status).FirstOrDefault().Trim(),
                    OrderDateTime = x.OrderDateTime
                }).Where(x => x.OrderId == orderId && x.StatusId == statusId).ToList();


                if (order.Any())
                {
                    return Ok(order);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        //PUT: order/delete/{id}
        [HttpPut]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var orderIds = (from x in dbx.tbl_Orders select x.OrderID).ToList();


                if (orderIds.Contains(id))
                {
                    var currentOrderStatus = (from o in dbx.tbl_Orders
                                              join s in dbx.tbl_Status
                                              on o.StatusID equals s.StatusID
                                              where o.OrderID == id
                                              select s.Status).FirstOrDefault().Trim();

                    var statusId = (from x in dbx.tbl_Status
                                    where x.Status == "Deleted" || x.Status == "deleted"
                                    select x.StatusID).SingleOrDefault();

                    if (currentOrderStatus == "Pending" || currentOrderStatus == "pending")
                    {
                        OrderRequest order = new OrderRequest();

                        order.StatusId = statusId;
                        // order.IsActive = "false";

                        var existingOrder = dbx.tbl_Orders.Where(x => x.OrderID == id).FirstOrDefault();

                        existingOrder.StatusID = order.StatusId;
                        // existingOrder.isActive = order.IsActive;

                        dbx.tbl_Orders.AddOrUpdate(existingOrder);
                        var result = dbx.SaveChanges();

                        if (result > 0)
                        {
                            return Ok("record deleted successfully");
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

        //PUT: order/edit/{id}

        [HttpPut]
        [Route("edit/{id}")]
        public IHttpActionResult Edit(int id, OrderRequest value)
        {
            try
            {
                var orderIds = (from x in dbx.tbl_Orders select x.OrderID).ToList();

                if (orderIds.Contains(id))
                {
                    var existingOrder = dbx.tbl_Orders.Where(x => x.OrderID == id).FirstOrDefault();

                    existingOrder.DeliveryAddress = value.DeliveryAddress;
                    existingOrder.StatusID = (from x in dbx.tbl_Status
                                              where x.Status == "Pending" || x.Status == "pending"
                                              select x.StatusID).FirstOrDefault();
                    //existingOrder.isActive = value.IsActive;

                    dbx.tbl_Orders.AddOrUpdate(existingOrder);
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
    }
}
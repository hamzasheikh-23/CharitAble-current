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
        charitable_dbEntities2 dbx = new charitable_dbEntities2();
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

                tbl_Orders order = new tbl_Orders
                {
                    NGO_ID = value.NGOId,
                    CaseID = value.CaseId,
                    ReplyID = value.ReplyId,
                    PaymentID = value.PaymentId,
                    DeliveryAddress = value.DeliveryAddress,
                    Amount = value.Amount,
                    OrderDateTime = value.OrderDateTime = DateTime.Now,
                    StatusID = value.StatusId = statusId
                };


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

        [HttpPost]
        [Route("response/post")]
        public IHttpActionResult ResponsePost(OrderRequest value)
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

                tbl_Orders order = new tbl_Orders
                {
                    DonorID = value.NGOId,
                    DonationID = value.DonationId,
                    ResponseID = value.ResponseId,
                    PaymentID = value.PaymentId,
                    DeliveryAddress = value.DeliveryAddress,
                    Amount = value.Amount,
                    OrderDateTime = value.OrderDateTime = DateTime.Now,
                    StatusID = value.StatusId = statusId
                };


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

                var order = (from x in dbx.tbl_Orders
                             join r in dbx.tbl_DonorReplies on x.ReplyID equals r.ReplyID
                             select new OrderRequest()
                             {
                                 OrderId = x.OrderID,
                                 NGOId = x.NGO_ID,
                                 NGOName = (from n in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on n.UserID equals u.UserID
                                            where n.NGO_ID == x.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonorId = x.DonorID,
                                 DonorName = (from d in dbx.tbl_DonorMaster
                                              join u in dbx.tbl_Users on d.UserID equals u.UserID
                                              where d.DonorID == x.DonorID
                                              select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 CaseId = x.CaseID,
                                 DonationId = x.DonationID,
                                 ReplyId = x.ReplyID,
                                 ResponseId = x.ResponseID,
                                 PaymentId = x.PaymentID,
                                 DropOffAddress = (from dr in dbx.NGOResponses
                                                   where dr.ResponseID == x.ResponseID
                                                   select dr.Address).FirstOrDefault().Trim(),
                                 PickupAddress = r.Address,
                                 DeliveryAddress = x.DeliveryAddress,
                                 Amount = x.Amount,
                                 StatusId = x.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == x.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 OrderDateTime = x.OrderDateTime
                             }).ToList();


                if (order.Any())
                {
                    ret = new { order, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest("'" + ex + ": " + ex.Message + "'");
            }
        }

        // GET: order/get?{ngoId}
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetO(int ngoId)
        {
            try
            {
                object ret = new { noData = true, status = "unsuccesfull request" };

                var order = (from x in dbx.tbl_Orders
                             join r in dbx.tbl_DonorReplies on x.ReplyID equals r.ReplyID
                             where x.NGO_ID == ngoId
                             select new OrderRequest()
                             {
                                 OrderId = x.OrderID,
                                 NGOId = x.NGO_ID,
                                 NGOName = (from n in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on n.UserID equals u.UserID
                                            where n.NGO_ID == x.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonorId = x.DonorID,
                                 DonorName = (from d in dbx.tbl_DonorMaster
                                              join u in dbx.tbl_Users on d.UserID equals u.UserID
                                              where d.DonorID == x.DonorID
                                              select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 CaseId = x.CaseID,
                                 DonationId = x.DonationID,
                                 ReplyId = x.ReplyID,
                                 ResponseId = x.ResponseID,
                                 PaymentId = x.PaymentID,
                                 DropOffAddress = (from dr in dbx.NGOResponses
                                                   where dr.ResponseID == x.ResponseID
                                                   select dr.Address).FirstOrDefault().Trim(),
                                 PickupAddress = r.Address,
                                 DeliveryAddress = x.DeliveryAddress,
                                 Amount = x.Amount,
                                 StatusId = x.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == x.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 OrderDateTime = x.OrderDateTime
                             }).ToList();

                if (order.Any())
                {
                    ret = new { order, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
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

                var order = (from x in dbx.tbl_Orders
                             join r in dbx.tbl_DonorReplies on x.ReplyID equals r.ReplyID
                             where x.OrderID == orderId
                             select new OrderRequest()
                             {
                                 OrderId = x.OrderID,
                                 NGOId = x.NGO_ID,
                                 NGOName = (from n in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on n.UserID equals u.UserID
                                            where n.NGO_ID == x.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonorId = x.DonorID,
                                 DonorName = (from d in dbx.tbl_DonorMaster
                                              join u in dbx.tbl_Users on d.UserID equals u.UserID
                                              where d.DonorID == x.DonorID
                                              select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 CaseId = x.CaseID,
                                 DonationId = x.DonationID,
                                 ReplyId = x.ReplyID,
                                 ResponseId = x.ResponseID,
                                 PaymentId = x.PaymentID,
                                 DropOffAddress = (from dr in dbx.NGOResponses
                                                   where dr.ResponseID == x.ResponseID
                                                   select dr.Address).FirstOrDefault().Trim(),
                                 PickupAddress = r.Address,
                                 DeliveryAddress = x.DeliveryAddress,
                                 Amount = x.Amount,
                                 StatusId = x.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == x.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 OrderDateTime = x.OrderDateTime
                             }).ToList();

                if (order.Any())
                {
                    ret = new { order, noData = false };
                    return Ok(ret);
                }

                return Ok(ret);
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

                var order = (from x in dbx.tbl_Orders
                             join r in dbx.tbl_DonorReplies on x.ReplyID equals r.ReplyID
                             where x.OrderID == orderId && x.StatusID == statusId
                             select new OrderRequest()
                             {
                                 OrderId = x.OrderID,
                                 NGOId = x.NGO_ID,
                                 NGOName = (from n in dbx.tbl_NGOMaster
                                            join u in dbx.tbl_Users on n.UserID equals u.UserID
                                            where n.NGO_ID == x.NGO_ID
                                            select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 DonorId = x.DonorID,
                                 DonorName = (from d in dbx.tbl_DonorMaster
                                              join u in dbx.tbl_Users on d.UserID equals u.UserID
                                              where d.DonorID == x.DonorID
                                              select u.FirstName + " " + u.LastName).FirstOrDefault().Trim(),
                                 CaseId = x.CaseID,
                                 DonationId = x.DonationID,
                                 ReplyId = x.ReplyID,
                                 ResponseId = x.ResponseID,
                                 PaymentId = x.PaymentID,
                                 DropOffAddress = (from dr in dbx.NGOResponses
                                                   where dr.ResponseID == x.ResponseID
                                                   select dr.Address).FirstOrDefault().Trim(),
                                 PickupAddress = r.Address,
                                 DeliveryAddress = x.DeliveryAddress,
                                 Amount = x.Amount,
                                 StatusId = x.StatusID,
                                 Status = (from y in dbx.tbl_Status
                                           where y.StatusID == x.StatusID
                                           select y.Status).FirstOrDefault().Trim(),
                                 OrderDateTime = x.OrderDateTime
                             }).ToList();

                if (order.Any())
                {
                    ret = new { order, noData = false };
                    return Ok(ret);
                }
                return Ok(ret);
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
                object ret = new { isSuccess = false };

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
                        OrderRequest order = new OrderRequest
                        {
                            StatusId = statusId
                        };
                        // order.IsActive = "false";

                        var existingOrder = dbx.tbl_Orders.Where(x => x.OrderID == id).FirstOrDefault();

                        existingOrder.StatusID = order.StatusId;
                        // existingOrder.isActive = order.IsActive;

                        dbx.tbl_Orders.AddOrUpdate(existingOrder);
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

        [HttpPut]
        [Route("edit")]
        public IHttpActionResult UpdateOrderStatus(int id, string status)
        {
            try
            {
                var orderIds = (from x in dbx.tbl_Orders select x.OrderID).ToList();

                if (orderIds.Contains(id))
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

                    OrderRequest value = new OrderRequest();

                    var existingOrder = dbx.tbl_Orders.Where(x => x.OrderID == id).FirstOrDefault();

                    existingOrder.StatusID = value.StatusId = statusId;

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
                return BadRequest(ex + " : '" + ex.Message + "'");
            }
        }
    }
}
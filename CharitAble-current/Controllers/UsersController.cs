using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Xml.Schema;
using CharitAble_current.Models;
using CharitAble_current.Requests;


namespace CharitAble_current.Controllers
{
    [RoutePrefix("user")]
    public class UsersController : ApiController
    {

        // POST user/login


        charitable_dbEntities2 dbx = new charitable_dbEntities2();

        [HttpPost]
        [Route("login")]
        public IHttpActionResult LoginResult(UserRequest value)
        {
            try
            {
                bool isSuccess = false;
                object ret = new
                {
                    isSucces = isSuccess,
                    code = "0",
                    msg = "Login Failed"
                };

                var result = dbx.tbl_Users.Where(x =>
                    x.Username.Equals(value.Username) && x.Password.Equals(value.Password)).FirstOrDefault();


                if (result != null)
                {

                    var userID =
                        (from x in dbx.tbl_Users
                         where x.Username == value.Username
                         select x.UserID).SingleOrDefault();


                    var userTypeId = result.UserTypeID;


                    isSuccess = true;

                    switch (result.UserTypeID)
                    {
                        case 1:
                            {
                                var adminID = (from x in dbx.tbl_Admin
                                               where x.UserID == userID
                                               select x.AdminID).SingleOrDefault();

                                var isActive = (from x in dbx.tbl_Admin
                                                where x.UserID == userID
                                                select x.isActive).SingleOrDefault();

                                ret = new
                                {
                                    isSuccess,
                                    userTypeId,
                                    adminID,
                                    userID,
                                    isActive,
                                    code = "1",
                                    msg = "Login Successful as Admin"
                                };

                                break;
                            }
                        case 2:
                            {
                                var donorID =
                                    (from x in dbx.tbl_DonorMaster
                                     where x.UserID == userID
                                     select x.DonorID).SingleOrDefault();

                                var isActive = (from x in dbx.tbl_DonorMaster
                                                where x.UserID == userID
                                                select x.isActive).SingleOrDefault();

                                ret = new
                                {
                                    isSuccess,
                                    userTypeId,
                                    donorID,
                                    userID,
                                    isActive,
                                    code = "2",
                                    msg = "Login Successful as Donor"
                                };

                                break;
                            }
                        case 3:
                            {
                                var ngoID =
                                    (from x in dbx.tbl_NGOMaster
                                     where x.UserID == userID
                                     select x.NGO_ID).SingleOrDefault();

                                var planID =
                                    (from x in dbx.tbl_NGOMaster
                                     where x.UserID == userID
                                     select x.PlanID).SingleOrDefault();

                                var isActive = (from x in dbx.tbl_NGOMaster
                                                where x.UserID == userID
                                                select x.isActive).SingleOrDefault();

                                ret = new
                                {
                                    isSuccess,
                                    userTypeId,
                                    ngoID,
                                    isActive,
                                    planID,
                                    userID,
                                    code = "3",
                                    msg = "Login Successful as NGO"
                                };

                                break;
                            }
                        default:
                            {
                                ret = new
                                {
                                    isSuccess = false,
                                    code = "unspecified",
                                    msg = "UserType not defined, False Login"
                                };

                                break;
                            }
                    }
                }

                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        //POST user/register

        [HttpPost]
        [Route("register")]
        public IHttpActionResult RegisterResult(UserRequest value)
        {
            bool isSuccess = false;

            try
            {
                object ret = new
                {
                    isSuccess,
                    code = "0",
                    status = "User not registered"
                };

                var isUsernameExist = (from x in dbx.tbl_Users
                                       where x.Username == value.Username
                                       select x.Username).SingleOrDefault();

                var isEmailExist = (from x in dbx.tbl_Users
                                    where x.Email == value.Email
                                    select x.Email).SingleOrDefault();

                if (!string.IsNullOrEmpty(isUsernameExist))
                {
                    ret = new
                    {
                        isSuccess,
                        code = "2",
                        status = "User not registered",
                        errMessage = "'" + isUsernameExist + "' already exist, try choosing a different username"
                    };

                    return Json(ret);
                }

                else if (!string.IsNullOrEmpty(isEmailExist))
                {
                    ret = new
                    {
                        isSuccess,
                        code = "2",
                        status = "User not registered",
                        errMessage = "Email address already registered"
                    };

                    return Json(ret);
                }

                tbl_Users user = new tbl_Users();

                user.FirstName = value.FirstName;
                user.LastName = value.LastName;
                user.Username = value.Username;
                user.Email = value.Email;
                user.Password = value.Password;
                user.ContactNumber = value.Contact;
                user.UserTypeID = value.UserTypeId;
                user.RegistrationDateTime = value.RegistrationDate = DateTime.Now;

                dbx.tbl_Users.AddOrUpdate(user);

                var result = dbx.SaveChanges();


                if (result != 0)
                {
                    var userTypeID =
                        (from x in dbx.tbl_Users
                         where x.Username == value.Username
                         select x.UserTypeID).SingleOrDefault();

                    isSuccess = true;

                    if (userTypeID == 1)
                    {
                        var userID = (from x in dbx.tbl_Users where x.Username == value.Username select x.UserID)
                            .SingleOrDefault();

                        tbl_Admin admin = new tbl_Admin();

                        admin.UserID = userID;
                        admin.isActive = "true";

                        dbx.tbl_Admin.AddOrUpdate(admin);
                        dbx.SaveChanges();
                    }

                    if (userTypeID == 2)
                    {
                        var userID = (from x in dbx.tbl_Users where x.Username == value.Username select x.UserID)
                            .SingleOrDefault();

                        tbl_DonorMaster donor = new tbl_DonorMaster();

                        donor.UserID = userID;
                        donor.isActive = "true";

                        dbx.tbl_DonorMaster.AddOrUpdate(donor);
                        dbx.SaveChanges();
                    }

                    if (userTypeID == 3)
                    {
                        var userID = (from x in dbx.tbl_Users where x.Username == value.Username select x.UserID)
                            .SingleOrDefault();

                        tbl_NGOMaster ngo = new tbl_NGOMaster();

                        ngo.UserID = userID;
                        ngo.isActive = "true";

                        dbx.tbl_NGOMaster.AddOrUpdate(ngo);
                        dbx.SaveChanges();
                    }

                    ret = new
                    {
                        isSuccess,
                        code = "1",
                        status = "success",
                    };
                }

                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // GET user/get

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetUsers()
        {
            bool isSuccess = false;

            try
            {
                var userList = dbx.tbl_Users.Select(x =>
                    new UserRequest()
                    {
                        UserId = x.UserID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Username = x.Username,
                        Email = x.Email,
                        Contact = (long)x.ContactNumber,
                        UserTypeId = x.UserTypeID,
                        RegistrationDate = (DateTime)x.RegistrationDateTime,
                        UpdateDate = (DateTime)x.UpdateDateTime
                    });

                if (userList.Any())
                {
                    return Json(userList);
                }

                object ret = new
                {
                    isSuccess,
                    code = "1",
                    status = "Failed"
                };
                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // GET user/get/id/{id}

        [HttpGet]
        [Route("get/id/{id}")]
        public IHttpActionResult GetUsers(int id)
        {
            try
            {
                var userList = dbx.tbl_Users.Select(x =>
                    new UserRequest()
                    {
                        UserId = x.UserID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Username = x.Username,
                        Email = x.Email,
                        Contact = (long)x.ContactNumber,
                        UserTypeId = x.UserTypeID,
                        RegistrationDate = (DateTime)x.RegistrationDateTime,
                        UpdateDate = (DateTime)x.UpdateDateTime
                    }).Where(x => x.UserId == id);
                return Json(userList);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }


        // GET user/get/usertype/{userType}

        [HttpGet]
        [Route("get/usertype/{userTypeId}")]
        public IHttpActionResult GetUsersByUserType(int userTypeId)
        {
            try
            {
                object ret = new { noData = true };

                if (userTypeId == 3)
                {
                    var ngoList = (from x in dbx.tbl_Users
                                   join n in dbx.tbl_NGOMaster on x.UserID equals n.UserID
                                   where x.UserTypeID == userTypeId
                                   select new UserRequest()
                                   {
                                       UserId = x.UserID,
                                       NgoId = (from n in dbx.tbl_NGOMaster
                                                join u in dbx.tbl_Users on n.UserID equals u.UserID
                                                where n.UserID == x.UserID
                                                select n.NGO_ID).FirstOrDefault(),
                                       FirstName = x.FirstName,
                                       LastName = x.LastName,
                                       Username = x.Username,
                                       Email = x.Email,
                                       Contact = x.ContactNumber,
                                       UserTypeId = x.UserTypeID,
                                       RegistrationDate = (DateTime)x.RegistrationDateTime,
                                       UpdateDate = (DateTime)x.UpdateDateTime,
                                       PlanId = n.PlanID,
                                       PlanName = (from y in dbx.tbl_SubscriptionPlan
                                                   where y.PlanID == n.PlanID
                                                   select y.PlanName).FirstOrDefault(),
                                       SubscriptionEndDate = n.SubscriptionEndDate,
                                       IsActive = n.isActive
                                   }).ToList();
                    ret = new { ngoList, noData = false };

                    return Ok(ret);
                }
                if (userTypeId == 2)
                {
                    var donorList = (from x in dbx.tbl_Users
                                     join d in dbx.tbl_DonorMaster on x.UserID equals d.UserID
                                     where x.UserTypeID == userTypeId
                                     select new UserRequest()
                                     {
                                         UserId = x.UserID,
                                         DonorId = d.DonorID,
                                         FirstName = x.FirstName,
                                         LastName = x.LastName,
                                         Username = x.Username,
                                         Email = x.Email,
                                         Contact = x.ContactNumber,
                                         UserTypeId = x.UserTypeID,
                                         RegistrationDate = (DateTime)x.RegistrationDateTime,
                                         UpdateDate = (DateTime)x.UpdateDateTime,
                                         CNIC = d.CNIC,
                                         Address = d.Address,
                                         IsActive = d.isActive
                                     }).ToList();

                    ret = new { donorList, noData = false };

                    return Ok(ret);
                }

                if (userTypeId == 1)
                {
                    var adminList = dbx.tbl_Users.Select(x =>
                    new UserRequest()
                    {
                        UserId = x.UserID,
                        AdminId = (from a in dbx.tbl_Admin
                                   join u in dbx.tbl_Users on a.UserID equals u.UserID
                                   where a.UserID == x.UserID
                                   select a.AdminID).FirstOrDefault(),
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Username = x.Username,
                        Email = x.Email,
                        Contact = x.ContactNumber,
                        UserTypeId = x.UserTypeID,
                        RegistrationDate = (DateTime)x.RegistrationDateTime,
                        UpdateDate = (DateTime)x.UpdateDateTime,
                        IsActive = (from a in dbx.tbl_Admin
                                    where a.UserID == x.UserID
                                    select a.isActive).FirstOrDefault(),
                    }).Where(x => x.UserTypeId == userTypeId);

                    ret = new { adminList, noData = false };

                    return Ok(ret);
                }
                else
                {
                    return Ok(ret);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT: user/edit/ngo?{id}&{isActive}
        [HttpPut]
        [Route("edit/ngo")]
        public IHttpActionResult UpdateNGO(int id, string isActive)
        {
            try
            {
                var ngoIds = (from x in dbx.tbl_NGOMaster select x.NGO_ID).ToList();

                if (ngoIds.Contains(id))
                {
                    UserRequest value = new UserRequest();

                    var existingNGO = dbx.tbl_NGOMaster.Where(x => x.NGO_ID == id).FirstOrDefault();


                    existingNGO.isActive = value.IsActive = isActive;

                    dbx.tbl_NGOMaster.AddOrUpdate(existingNGO);
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

        //PUT: user/edit/donor?{id}&{isActive}
        [HttpPut]
        [Route("edit/donor")]
        public IHttpActionResult UpdateDonor(int id, string isActive)
        {
            try
            {
                var donorIds = (from x in dbx.tbl_DonorMaster select x.DonorID).ToList();

                if (donorIds.Contains(id))
                {
                    UserRequest value = new UserRequest();

                    var existingDonor = dbx.tbl_DonorMaster.Where(x => x.DonorID == id).FirstOrDefault();


                    existingDonor.isActive = value.IsActive = isActive;

                    dbx.tbl_DonorMaster.AddOrUpdate(existingDonor);
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

        //PUT: user/edit/admin?{id}&{isActive}
        [HttpPut]
        [Route("edit/admin")]
        public IHttpActionResult UpdateAdmin(int id, string isActive)
        {
            try
            {
                var adminIds = (from x in dbx.tbl_Admin select x.AdminID).ToList();

                if (adminIds.Contains(id))
                {
                    UserRequest value = new UserRequest();

                    var existingAdmin = dbx.tbl_Admin.Where(x => x.AdminID == id).FirstOrDefault();


                    existingAdmin.isActive = value.IsActive = isActive;

                    dbx.tbl_Admin.AddOrUpdate(existingAdmin);
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

        [HttpGet]
        [Route("users/count")]
        public IHttpActionResult Get(int userType)
        {
            try
            {
                object ret = new
                {
                    isSuccess = false,
                    msg = "no user found"
                };

                var userList = (from x in dbx.tbl_Users
                                where x.UserTypeID == userType
                                select x).ToList();

                if (userList.Any())
                {
                    var count = userList.Count;
                    ret = new
                    {
                        isSuccess = true,
                        msg = "users found",
                        count
                    };
                    return Ok(ret);
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("users/count")]
        public IHttpActionResult Get()
        {
            try
            {
                object ret = new
                {
                    isSuccess = false,
                    msg = "no user found"
                };

                var userList = (from x in dbx.tbl_Users
                                select x).ToList();

                if (userList.Any())
                {
                    var count = userList.Count;
                    ret = new
                    {
                        isSuccess = true,
                        msg = "users found",
                        count
                    };
                    return Ok(ret);
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

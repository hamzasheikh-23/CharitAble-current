using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using CharitAble_current.Models;
using CharitAble_current.Requests;


namespace CharitAble_current.Controllers
{
    [RoutePrefix("user")]
    public class UsersController : ApiController
    {

        // POST user/login


        charitable_dbEntities1 dbx = new charitable_dbEntities1();
        [HttpPost]
        [Route("login")]
        public IHttpActionResult LoginResult(UserRequest value)
        {
            bool isSuccess;
            object ret = new
            {
                isSucces = false,
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


                switch (result.UserTypeID)
                {
                    case 1:
                    {
                        var adminID = (from x in dbx.tbl_Admin
                            where x.UserID == userID
                            select x.AdminID).SingleOrDefault();
                           
                            ret = new
                            {
                                isSuccess = true,
                                userTypeId,
                                adminID,
                                userID,
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

                            ret = new
                            {
                                isSuccess = true,
                                userTypeId,
                                donorID,
                                userID,
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

                            ret = new
                            {
                                isSuccess = true,
                                userTypeId,
                                ngoID,
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

        //POST user/register

        [HttpPost]
        [Route("register")]
        public IHttpActionResult RegisterResult(UserRequest value)
        {

            try
            {

                object ret = new
                {
                    code = "0",
                    status = "User not registered"
                };

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

                    if (userTypeID == 1)
                    {
                        var userID = (from x in dbx.tbl_Users where x.Username == value.Username select x.UserID)
                            .SingleOrDefault();

                        tbl_Admin admin = new tbl_Admin();

                        admin.UserID = userID;

                        dbx.tbl_Admin.AddOrUpdate(admin);
                        dbx.SaveChanges();
                    }

                    if (userTypeID == 2)
                    {
                        var userID = (from x in dbx.tbl_Users where x.Username == value.Username select x.UserID)
                            .SingleOrDefault();

                        tbl_DonorMaster donor = new tbl_DonorMaster();

                        donor.UserID = userID;

                        dbx.tbl_DonorMaster.AddOrUpdate(donor);
                        dbx.SaveChanges();
                    }

                    if (userTypeID == 3)
                    {
                        var userID = (from x in dbx.tbl_Users where x.Username == value.Username select x.UserID)
                            .SingleOrDefault();

                        tbl_NGOMaster ngo = new tbl_NGOMaster();

                        ngo.UserID = userID;

                        dbx.tbl_NGOMaster.AddOrUpdate(ngo);
                        dbx.SaveChanges();
                    }

                    ret = new
                    {
                        code = "1",
                        status = "success",
                    };
                }

                return Json(ret);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET user/get

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetUsers()
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
            return Json(userList);
        }

        // GET user/get/id/{id}

        [HttpGet]
        [Route("get/id/{id}")]
        public IHttpActionResult GetUsers(int id)
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

        // GET user/get/usertype/{userType}

        [HttpGet]
        [Route("get/usertype/{userTypeId}")]
        public IHttpActionResult GetUsersByUserType(int userTypeId)
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
                }).Where(x => x.UserTypeId == userTypeId);
            return Json(userList);
        }


    }

}

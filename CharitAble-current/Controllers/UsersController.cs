﻿using System;
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


        charitable_dbEntities1 dbx = new charitable_dbEntities1();

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

                                ret = new
                                {
                                    isSuccess,
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
                                    isSuccess,
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
                                    isSuccess,
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
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }



    }

}

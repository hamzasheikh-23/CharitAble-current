﻿using System;
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

            object ret = new
            {
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
                var donorID =
                    (from x in dbx.tbl_DonorMaster
                     where x.UserID == userID
                     select x.DonorID).SingleOrDefault();

                switch (result.UserTypeID)
                {
                    case 1:
                        {
                            ret = new
                            {
                                userID,
                                code = "1",
                                msg = "Login Successful as Admin"
                            };

                            break;
                        }
                    case 2:
                        {
                            ret = new
                            {
                                donorID,
                                userID,
                                code = "2",
                                msg = "Login Successful as Donor"
                            };

                            break;
                        }
                    case 3:
                        {
                            ret = new
                            {
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
                ret = new
                {
                    code = "1",
                    status = "success",
                };
            }

            return Json(ret);
        }

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetUsers()
        {
            var userList = dbx.tbl_Users.Select(x =>
                new UserRequest()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username,
                    Email = x.Email,
                    Contact = (long)x.ContactNumber,
                    UserTypeId = x.UserTypeID,
                    // RegistrationDate = (DateTime)x.RegistrationDateTime
                }).ToList();
            return Json(userList);
        }

    }

}

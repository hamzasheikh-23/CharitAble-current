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
        public IHttpActionResult LoginResult(LoginRequest value)
        {

            object ret = new
            {
                code = "0",
                msg = "Login Failed"
            };

            var result = dbx.tbl_Users.Where(x =>
                (x.Username.Equals(value.usernameOrEmail) && x.Password.Equals(value.password)) ||
                (x.Email.Equals(value.usernameOrEmail) && x.Password.Equals(value.password))).FirstOrDefault();


            if (result != null)
            {
                switch (result.UserTypeID)
                {
                    case 1:
                        {
                            ret = new
                            {
                                code = "1",
                                msg = "Login Successful as Admin"
                            };

                            break;
                        }
                    case 2:
                        {
                            ret = new
                            {
                                code = "2",
                                msg = "Login Successful as Donor"
                            };

                            break;
                        }
                    case 3:
                        {
                            ret = new
                            {
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
        public IHttpActionResult RegisterResult(RegisterRequest value)
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
            user.RegistrationDateTime = value.RegistrationDate;

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

    }

}

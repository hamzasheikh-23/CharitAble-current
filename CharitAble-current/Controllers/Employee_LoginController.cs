using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CharitAble_current.Models;

namespace CharitAble_current.Controllers
{
    [RoutePrefix("api/employee")]
    public class Employee_LoginController : ApiController
    {
        private charitable_dbEntities db = new charitable_dbEntities();

        [Route("login")]
        [HttpPost]
        public IHttpActionResult employeeLogin(Login login)
        {
            var log = db.tbl_Users.FirstOrDefault(x => (
                x.Email == login.Email && x.Password == login.Password)
                || (x.Username == login.Username && x.Password ==
                login.Password));

            if (log == null)
            {
                return Ok(new
                {
                    status = 401, isSuccess = false,
                    message = "Invalid User"
                });
            }
            else
            {
                return Ok(new
                {
                    status = 200, isSuccess = true,
                    message = "User Login successfully", UserDetails = log
                });
            }
        }

        [Route("InsertEmployee")]
        [HttpPost]
        public object insertEmployee(Register reg)
        {
            try
            {
                tbl_Users us = new tbl_Users();
                if (us.UserID == 0)
                {
                    us.FirstName = reg.FirstName;
                    us.LastName = reg.LastName;
                    us.Username = reg.Username;
                    us.Email = reg.Email;
                    us.Password = reg.Password;
                    us.ContactNumber = reg.ContactNumber;
                }

                db.tbl_Users.Add(us);
                db.SaveChanges();
                return new Response
                {
                    Status = "Success", Message = "Record Successfully Saved"
                };
            }
            catch (Exception)
            {
                throw;
            }

            return new Response
            {
                Status = "Error", Message = "Invalid Data."
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace CharitAble_current.Requests
{
    public class UserRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Username { get; set; }
        public int NgoId { get; set; }
        public int DonorId { get; set; }
        public int AdminId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long Contact { get; set; }
        public int UserTypeId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string IsActive { get; set; }
    }
}
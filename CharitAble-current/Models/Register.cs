using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CharitAble_current.Models
{
    public class Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double ContactNumber { get; set; }
        public string  UserType { get; set; }
        public string Image { get; set; }
    }
}
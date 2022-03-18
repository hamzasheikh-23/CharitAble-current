using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharitAble_current.Requests
{
    public class LoginRequest
    {
        public string usernameOrEmail { get; set; }
        public string password { get; set; }

    }
}
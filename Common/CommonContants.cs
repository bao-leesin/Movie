using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Common
{
    public static class CommonContants
    {
        public static string LOGIN_SESSION = "LOGIN_SESSION";
        public enum Role
        {
            ADMIN = 1,
            CLIENT = 2
        };
    }
}
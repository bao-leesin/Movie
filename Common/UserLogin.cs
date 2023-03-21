using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Common
{
    [Serializable]
    public class UserLogin
    {
        public string Username { set; get; }

        public string Role { set; get; }
       
        public UserLogin(string Username, string Role)
        {
            this.Username = Username;
            this.Role = Role;
       
        }
    }
}
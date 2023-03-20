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
        public string Password { set; get; }
        public bool isAdmin { set; get; }
        public UserLogin(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
       
        }
    }
}
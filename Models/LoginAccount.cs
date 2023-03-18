using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class LoginAccount
    {
        [Required(ErrorMessage = "Ayyo")]
        public string Username { set; get; }
        [Required(ErrorMessage = "...")]
        public string Password { set; get; }
    }
}
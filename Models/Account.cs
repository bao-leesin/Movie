using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;


namespace Movie.Models
{
    public class Account
    {
        [StringLength(12)]
        public string Username { set; get; }

        [StringLength(15)]
        public string Password { set; get; }

        [StringLength(45)]
        public string Fullname { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Ticket : BookingShowtime
    {
        public string Username { get; set; }
        public int IdChair { get; set; }
    }
}
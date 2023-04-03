using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Ticket : BookingShowtime
    {
        public string Username { get; set; }
        public List<int> IdChair { get; set; }
        public int idShowtime { get; set; }
    }
}
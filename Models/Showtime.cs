using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Showtime : Film
    {
        public int MovieId { set; get; }
        public string RoomName { set; get; }
        public string Type { set; get; }
        public DateTime StartTime { set; get; }
       

    }

    public class BookingShowtime : Showtime {
        public string City { set; get; }
        public string MovieTheaterName { set; get; }
        public string MovieName { set; get; }
        
    }
}
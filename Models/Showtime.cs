using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Showtime
    {
        public int id { set; get; }
        public int movieId { set; get; }
        public int roomId { set; get; }
        public string type { set; get; }
        public DateTime startTime { set; get; }
        public string showDay { set; get; }

        //public Showtime(int id,int id_movie, int id_room, string type, DateTime start_time)
        //{
        //    this.id = id;
        //    this.id_movie = id_movie;   
        //    this.id_room = id_room;
        //    this.type = type;
        //    this.start_time = start_time;
        //}

    }

    public class BookingShowtime : Showtime {
        public string city { set; get; }
        public string movieTheaterName { set; get; }
        public string roomName { set; get; }
        public string movieName { set; get; }

    }
}
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
        public int idMovie { set; get; }
        public int idRoom { set; get; }
        public string type { set; get; }
        public string startTime { set; get; }
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
        public string nameMovieTheater { set; get; }
        public string nameRoom { set; get; }
        public string nameMovie { set; get; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Showtime
    {
        
        public int id { set; get; }
        public int id_movie { set; get; }
        public string id_room { set; get; }
        public string type { set; get; }
        public DateTime start_time { set; get; }



    }
}
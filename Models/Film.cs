using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Film 
    {
        public int Id { get; set; }
        public string FilmName { get; set; }
        public int Duration { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
    }
}
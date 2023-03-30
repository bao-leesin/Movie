using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Chair
    {
        public int Id { get; set; }
        public string ChairNumber { get; set; }
        public string Tier { get; set; }  
        public int OverSpending { get; set; }

    }
}
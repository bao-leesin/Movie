using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Models
{
    public class Chair
    {
        public int Id { get; set; }
        public string Tier { get; set; }  
        public int OverSpending { get; set; }

    }

    public class ChairGroup : Chair
    {
        public List<string> Chairs { get; set; }
    }
}
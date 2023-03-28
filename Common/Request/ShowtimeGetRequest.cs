using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie.Common.Request
{

    public class ShowtimeGetRequest
    {
        int? idFilm { set; get; }
        string cityName { set; get; }   
        DateTime showDayInput { set; get; }
        string type { set; get; }   
    }
}
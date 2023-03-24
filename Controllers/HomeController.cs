using Movie.DAO;
using Movie.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie.Controllers
{
    public class HomeController : Controller
    {

        

        public ActionResult Index()
        {
            var dao = new MovieDao();
            var films = dao.getAllMovie();
            return View(films);
        }
 
        public ActionResult bookShowTimeByIdFilm(int? idFilm)
        {
            var dao = new ShowtimeDao();
            var showtimes = dao.getBookingShowtimeById(idFilm);
            return View(showtimes);
        }
        public ActionResult BookShowTimeByDay()
        {
            return View();
        }

        
    }
}
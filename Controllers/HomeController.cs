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
 
        public ActionResult bookShowTimeByIdFilm(int? idFilm, string cityName, DateTime showDayInput, string type)
        {
            var dao = new ShowtimeDao();

            ViewBag.cities = dao.GetCitiesByIdMovie(idFilm);
            ViewBag.showDays = dao.getShowDaysByIdMovie(idFilm);
            ViewBag.types = dao.getTypeCinemaByIdMovie(idFilm);

            //if(showDayInput == null) { showDayInput = ViewBag.showDays }
            //if(cityName == null) { cityName = ""}

            ViewData["show time"] = dao.getBookingShowtime(idFilm,  cityName,  showDayInput,  type);
             
            return View();
        }
        public ActionResult BookShowTimeByDay()
        {
            return View();
        }

        
    }
}
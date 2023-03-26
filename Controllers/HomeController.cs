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

            var cities = ViewBag.cities = dao.getCitiesByIdMovie(idFilm);
            var showDays = ViewBag.showDays = dao.getShowDaysByIdMovie(idFilm);
            var types = ViewBag.types = dao.getTypeCinemaByIdMovie(idFilm);

            string cityName = cities.FirstOrDefault();
            DateTime showDayInput = showDays.FirstOrDefault();
            string type = types.FirstOrDefault();

            ViewData["show time"] = dao.getBookingShowtime(idFilm,  cityName,  showDayInput,  type);
             
            return View();
        }

        public void filterShowTime(int? idFilm, string cityName, DateTime showDayInput, string type)
        {
            ViewData["show time"] = new ShowtimeDao().getBookingShowtime(idFilm, cityName, showDayInput, type);
            Redirect("bookShowTimeByIdFilm");
        }
        public ActionResult BookShowTimeByDay()
        {
            return View();
        }

    }
}
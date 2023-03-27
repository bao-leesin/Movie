using Movie.DAO;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Movie.Controllers
{
    public class HomeController : BaseController
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

            var showDays = dao.getShowDaysByIdMovie(idFilm);
            var cities = dao.getCitiesByIdMovie(idFilm);
            var types = dao.getTypeCinemaByIdMovie(idFilm);

            ViewData["cities"] = cities;
            ViewData["showDays"] = showDays;
            ViewData["types"] = types;

            DateTime showDayInput =  DateTime.Now ;
            string type = "";
            string cityName = "";


            if (showDays.FirstOrDefault() != null)
            { 
                showDayInput = showDays.FirstOrDefault().startTime; 
            }

            if (types.FirstOrDefault() != null)
            { 
                type = types.FirstOrDefault().Type; 
            }

            if (cities.FirstOrDefault() != null)
            {
                cityName = cities.FirstOrDefault().City;
            }

            var showTimes = dao.getBookingShowtime(idFilm, cityName, showDayInput, type);
            ViewData["show time"] = showTimes;

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
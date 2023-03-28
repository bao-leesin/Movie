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
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var dao = new MovieDao();
            var films = dao.getAllMovie();
            return View(films);
        }
 
        public ViewResult bookShowTimeByIdFilm(int? idFilm)
        {
            //var dao = new ShowtimeDao();

            //var showDays = dao.getShowDaysByIdMovie(idFilm);
            //var cities = dao.getCitiesByIdMovie(idFilm);
            //var types = dao.getTypeCinemaByIdMovie(idFilm);

            TempData["idFilm"] = idFilm;
            List<BookingShowtime> every = new List<BookingShowtime>();
            every.Add(new BookingShowtime
            {
                Id = 1,
                movieId = 1,
                City = "Hà Nội",
                movieName = "Hê",
                movieTheaterName = "CGv",
                roomId = 1,
                startTime = DateTime.Now,
                Type = "2d"
            });
            every.Add(new BookingShowtime
            {
                Id = 2,
                movieId = 1,
                City = "Hà Nội",
                movieName = "Hê",
                movieTheaterName = "CGv",
                roomId = 1,
                startTime = DateTime.Now,
                Type = "2d"
            });
            every.Add(new BookingShowtime
            {
                Id = 3,
                movieId = 1,
                City = "Hà Nội",
                movieName = "Hê",
                movieTheaterName = "CGv",
                roomId = 1,
                startTime = DateTime.Now,
                Type = "2d"
            });

            ViewData["cities"] = every;
            ViewData["showDays"] = every;
            ViewData["types"] = every;

            string showDayInput =  "" ;
            string type = "";
            string cityName = "";


            //if (showDays.FirstOrDefault() != null)
            //{ 
            //    showDayInput = showDays.FirstOrDefault().startTime; 
            //}

            //if (types.FirstOrDefault() != null)
            //{ 
            //    type = types.FirstOrDefault().Type; 
            //}

            //if (cities.FirstOrDefault() != null)
            //{
            //    cityName = cities.FirstOrDefault().City;
            //}

            //var showTimes = dao.getBookingShowtime(idFilm, cityName, showDayInput, type);

            ViewData["show time"] = every;

            return View();
        }

        [Route("{cityName}/{showDayInput}/{type}")]
        [HttpGet]
        public PartialViewResult filterShowTime(String showDayInput,string cityName, string type)
        {
            int idFilm = (int)TempData["idFilm"];
            ViewData["show time"] = new ShowtimeDao().getBookingShowtime(idFilm, cityName, showDayInput, type);
            return PartialView("shared/_ContainerCinema");
        }
        public ActionResult BookShowTimeByDay()
        {
            return View();
        }

    }
}
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
        
        [HttpGet]
        public ViewResult bookShowTimeByIdFilm(int? idFilm)
        {
            var dao = new ShowtimeDao();
            var showDays = dao.getShowDaysByIdMovie(idFilm);
            var cities = dao.getCitiesByIdMovie(idFilm);
            var types = dao.getTypeCinemaByIdMovie(idFilm);

            //List<BookingShowtime> every = new List<BookingShowtime>();
            //every.Add(new BookingShowtime
            //{
            //    Id = 1,
            //    movieId = 1,
            //    City = "Hà Nội",
            //    movieName = "Hê",
            //    movieTheaterName = "CGv",
            //    roomId = 1,
            //    startTime = DateTime.Now,
            //    Type = "2d"
            //});
            //every.Add(new BookingShowtime
            //{
            //    Id = 2,
            //    movieId = 1,
            //    City = "Hà Nội",
            //    movieName = "Hê",
            //    movieTheaterName = "CGv",
            //    roomId = 1,
            //    startTime = DateTime.Now,
            //    Type = "2d"
            //});
            //every.Add(new BookingShowtime
            //{
            //    Id = 3,
            //    movieId = 1,
            //    City = "Hà Nội",
            //    movieName = "Hê",
            //    movieTheaterName = "CGv",
            //    roomId = 1,
            //    startTime = DateTime.Now,
            //    Type = "2d"
            //});

            //ViewData["cities"] = every;
            //ViewData["showDays"] = every;
            //ViewData["types"] = every;
            //ViewData["show time"] = every;


            ViewData["cities"] = cities;
            ViewData["showDays"] = showDays;
            ViewData["types"] = types;

            return View();
        
        }

        
        public ActionResult filterShowTime(string cityName, DateTime showDayInput, string type,int IdFilm)
        {
            List<BookingShowtime> showtimes = new ShowtimeDao().getBookingShowtime(IdFilm, cityName, showDayInput, type);
            return PartialView("_ContainerCinema",showtimes);
        }

        [HttpGet]
        public ActionResult bookChairs(int idShowTime)
        {
            TempData.Clear();

            return View();
        }

        [HttpPost]
        public ActionResult bookChairs()
        {
            return View();
        }
    }
}
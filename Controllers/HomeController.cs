using Movie.DAO;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Dynamic;
using Microsoft.Ajax.Utilities;

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

            //ViewData["cities"] = every;
            //ViewData["showDays"] = every;
            //ViewData["types"] = every;

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
        public ActionResult bookChairs()
        {
            TempData.Clear();

            //var roomDao = new RoomDao();

            //Room room = roomDao.getChairsOfRoom(idShowtime);
            //List<Chair> soldChairs = roomDao.getSoldChairIdList(idShowtime);
            //int chairQuantity = room.ChairQuantity;

            List<Chair> soldChairs = new List<Chair>();
            soldChairs.Add(new Chair() { Id = 1 });
            soldChairs.Add(new Chair() { Id = 11 });
            soldChairs.Add(new Chair() { Id = 31 });
            soldChairs.Add(new Chair() { Id = 41 });

            /* Chưa tìm được cách nhẹ nhàng để hiện tier */

            //List<Chair> normalTierChairs = new List<Chair>();
            //normalTierChairs.Add(new Chair() { Id = 1 });
            //normalTierChairs.Add(new Chair() { Id = 2 });
            //normalTierChairs.Add(new Chair() { Id = 3 });
            //normalTierChairs.Add(new Chair() { Id = 4 });

            //List<Chair> vipTierChairs = new List<Chair>();
            //vipTierChairs.Add(new Chair() { Id = 13 });
            //vipTierChairs.Add(new Chair() { Id = 14 });
            //vipTierChairs.Add(new Chair() { Id = 15 });
            //vipTierChairs.Add(new Chair() { Id = 16 });

            int chairQuantity = 80;

            ViewBag.ChairQuantity = chairQuantity;
            ViewBag.NumberOfRow = 10;
            ViewBag.NormalOverspending = 10;
            ViewBag.VipOverspending = 20;

            dynamic mymodel = new ExpandoObject();
            mymodel.SoldChairs = soldChairs;
            //mymodel.NormalTierChairs = normalTierChairs;
            //mymodel.VipTierChairs = vipTierChairs;

            return View(mymodel);
        }

        public JsonResult dístributeChairTier()
        {
            int a=0;
            return Json(a);
        }

        [HttpGet]
        public JsonResult calculatePrice(int[] idChairs)
        {
            
            int basePrice = 1;
            int overspending = 2; 
            int price = basePrice + overspending*idChairs.Length;

            return Json(price);
        }

        //[HttpPost]
        //public ActionResult bookChairs()
        //{
        //    return View();
        //}
    }
}
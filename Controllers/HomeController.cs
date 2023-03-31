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
using System.Web.Helpers;

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
        public ActionResult bookChairs(int idShowtime)
        {
            ViewData.Clear();

            var roomDao = new RoomDao();
            var chairDao = new ChairDao();

            Room room = roomDao.getChairsOfRoom(idShowtime);

            int chairQuantity;

            if (room != null)
            {
             chairQuantity = room.ChairQuantity;
            }
            else
            {
             chairQuantity = 0;
            }
            

            List<int> soldChairs = chairDao.getSoldChairList(idShowtime);


            //int chairQuantity = 100;

            //List<Chair> soldChairs = new List<Chair>();
            //soldChairs.Add(new Chair() { Id = 1 });
            //soldChairs.Add(new Chair() { Id = 11 });
            //soldChairs.Add(new Chair() { Id = 31 });
            //soldChairs.Add(new Chair() { Id = 41 });

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


            int numberOfRow = 10;
            int numberOfColumn = chairQuantity / numberOfRow;

            char[] alphabet = new char[numberOfColumn];

            for(int i = 65; i < 65 + numberOfColumn; i++)
            {
                alphabet[i-65] = (char)i;
            }


            ViewBag.Alphabet = alphabet;
            ViewBag.ChairQuantity = chairQuantity;
            ViewBag.NumberOfRow = numberOfRow;
            ViewBag.NumberOfColumn = numberOfColumn;

            dynamic mymodel = new ExpandoObject();
            mymodel.SoldChairs = soldChairs;


            return View(mymodel);
        }
        [HttpGet]
        public JsonResult distributeChairTier()
        {
            var dao = new ChairDao();
            var listTierChair = dao.getTierChair();

            List<ChairGroup> chairGroup = new List<ChairGroup>();

            foreach (var tier in listTierChair)
            {
                chairGroup.Add(
                new ChairGroup()
                {
                    Tier = tier.ToString(),
                    Chairs = dao.getChairsByTier(tier.ToString())
                });
            }

            return Json(chairGroup,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult calculatePrice(int[] selectedChairs)
        {

            var chairDao = new ChairDao();
            var chairPrices = chairDao.getChairPrice();



            return Json(chairPrices);
        }

        //[HttpPost]
        //public ActionResult bookChairs()
        //{
        //    return View();
        //}
    }
}
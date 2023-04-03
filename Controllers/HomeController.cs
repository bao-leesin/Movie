using Movie.DAO;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Dynamic;
using Newtonsoft.Json;
using System.Net;
using System.Web.Services.Description;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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

            ViewData["cities"] = cities;
            ViewData["showDays"] = showDays;
            ViewData["types"] = types;

            return View();

        }

        public ActionResult filterShowTime(string cityName, DateTime showDayInput, string type, int IdFilm)
        {
            List<BookingShowtime> showtimes = new ShowtimeDao().getBookingShowtime(IdFilm, cityName, showDayInput, type);
            return PartialView("_ContainerCinema", showtimes);
        }

        [HttpGet]
        public ActionResult bookChairs(int idShowtime)
        {
            ViewData.Clear();

            var roomDao = new RoomDao();
            var chairDao = new ChairDao();
            var showTimeDao = new ShowtimeDao();

            int chairQuantity = roomDao.getChairsOfRoom(idShowtime);

            List<int> soldChairs = chairDao.getSoldChairList(idShowtime);
            BookingShowtime ShowTimeInfo = showTimeDao.GetShowtimeDetail(idShowtime);

            /* Có thể đưa số hàng số cột vào CSDL*/
            int numberOfRow = 10;
            int numberOfColumn = chairQuantity / numberOfRow;

            char[] alphabet = new char[numberOfColumn];

            for (int i = 65; i < 65 + numberOfColumn; i++)
            {
                alphabet[i - 65] = (char)i;
            }

            ViewBag.Alphabet = alphabet;
            ViewBag.ChairQuantity = chairQuantity;
            ViewBag.NumberOfRow = numberOfRow;
            ViewBag.NumberOfColumn = numberOfColumn;

            dynamic mymodel = new ExpandoObject();
            mymodel.SoldChairs = soldChairs;
            mymodel.ShowTimeInfo = ShowTimeInfo;


            return View(mymodel);
        }

        [HttpGet]
        public JsonResult getChairTiers()
        {
            var dao = new ChairDao();
            var listChairTier = dao.getTierChair().ToArray();
            return Json(listChairTier, JsonRequestBehavior.AllowGet);
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
            return Json(chairGroup, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult calculatePrice(string tierWithChairs,string jsonChairIds) 
        {
            var chairDao = new ChairDao();
            int total = 0;

            JObject listSelectedChairByTier = JObject.Parse(tierWithChairs);
            JArray jArray = JArray.Parse(jsonChairIds);
            var selectedChairs = jArray.ToObject<List<int>>();


            foreach (JProperty property in listSelectedChairByTier.Properties()) { 
                string tier = property.Name;
                int quantity = (int)property.Value;
                int chairPrice = chairDao.getChairPrice(tier) ;
                total += chairPrice*quantity;
            }

            ViewData["Chair Price"] = total;
            ViewData["Selected Chair"] = selectedChairs;

            return Json(total);
        }

        public ActionResult bookTicket(BookingShowtime bookingShowtime)
        {
            var dao = new ShowtimeDao();
            int price = (int)ViewData["Chair Price"];
            List<int> selectedChairs = ViewData["Selected Chair"] as List<int> ;

            var ticket = new Ticket
            {
                MovieName = bookingShowtime.MovieName,
                MovieTheaterName = bookingShowtime.MovieTheaterName,
                StartTime = bookingShowtime.StartTime,
                RoomName = bookingShowtime.RoomName,
                Price = price,
                Username = Convert.ToString(Session["username"]),
                IdChair = selectedChairs
            };

            return View(ticket);
        }

    }
}
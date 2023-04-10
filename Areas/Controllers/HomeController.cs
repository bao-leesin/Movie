using Movie.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home

        [Authorize(Roles = "ADMIN")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
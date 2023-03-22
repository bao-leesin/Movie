using Movie.Common;
using Movie.DAO;
using Movie.Models;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web.Mvc;

namespace Movie.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginAccount respone)
        {
            var dao = new AccountDao();
            var result = dao.LoginUser(respone.Username, respone.Password);

          
            switch (result)
            {
                //Tất cả các trường hợp như không có tài khoản hoặc lỗi DB đều trả về case 0
                case 0:
                    ModelState.AddModelError("", "Failed");
                    return View();

                case (int)CommonContants.Role.ADMIN:
                    Session[CommonContants.LOGIN_SESSION] = new UserLogin(respone.Username, "Admin");
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                case (int)CommonContants.Role.CLIENT:
                    Session[CommonContants.LOGIN_SESSION] = new UserLogin(respone.Username, "Client");
                    return RedirectToAction("Index", "Home");
                 
            }
          return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        public ActionResult Logout() {
            Session[CommonContants.LOGIN_SESSION] = null;
            return View(); 
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}
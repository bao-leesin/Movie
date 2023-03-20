using Movie.Common;
using Movie.DAO;
using Movie.Models;
using Oracle.ManagedDataAccess.Client;
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
        public ActionResult Index(LoginAccount data)
        {
            var dao = new UserDao();
            var result = dao.LoginUser(data.Username, data.Password);

            //Nếu độ dài chuỗi trả về ko null thì kiểm tra role
            switch (result)
            {
                //Tất cả các trường hợp như không có tài khoản hoặc lỗi DB đều trả về case 0
                case 0:
                    ModelState.AddModelError("", "Failed");
                    return View();

                case (int)CommonContants.Role.ADMIN:
                    Session["ROLE"] = CommonContants.Role.ADMIN;
                    Session[CommonContants.LOGIN_SESSION] = new UserLogin(data.Username, data.Password);
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                case (int)CommonContants.Role.CLIENT:
                    Session["ROLE"] = CommonContants.Role.CLIENT;
                    Session[CommonContants.LOGIN_SESSION] = new UserLogin(data.Username, data.Password);
                    return RedirectToAction("Index", "Home");
                 
            }
            return View();
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
using Movie.Common;
using Movie.DAO;
using Movie.Models;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Index(LoginAccount request)
        {
            var dao = new AccountDao();
            var result = dao.LoginUser(request.Username, request.Password);

            switch (result)
            {
                //Tất cả các trường hợp như không có tài khoản hoặc lỗi DB đều trả về case 0
                case 0:
                    ModelState.AddModelError("", "Failed");
                    return View();

                case (int)CommonContants.Role.ADMIN:
                    FormsAuthentication.SetAuthCookie
                    Session[CommonContants.LOGIN_SESSION] = new UserLogin(request.Username, "Admin");
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                case (int)CommonContants.Role.CLIENT:
                    Session[CommonContants.LOGIN_SESSION] = new UserLogin(request.Username, "Client");
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
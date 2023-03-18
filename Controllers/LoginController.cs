using Movie.Common;
using Movie.DAO;
using Movie.Models;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;
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

            if (result)
            {
                var loginSesssion = new UserLogin(data.Username, data.Password);
                // Thêm mới hoặc thay thế nếu đã có session cho student
                Session[CommonContants.LOGIN_SESSION] = loginSesssion;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Failed");
            }
            return View();
        }
        public ActionResult Logout() { return View(); }
    }
}
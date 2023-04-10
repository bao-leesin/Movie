using Movie.Common;
using Movie.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;

namespace Movie.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sess = (UserLogin)Session[CommonContants.LOGIN_SESSION];
            if (sess == null)
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                    new RouteValueDictionary(
                    new
                    {
                        action = "Index",
                        Controller = "Login"
                    }));
            }
            base.OnActionExecuting(filterContext);

        }

        
        



        



     
    }
}
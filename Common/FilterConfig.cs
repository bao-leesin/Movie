using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie.Common
{
    public class FilterConfig
    {
        public static void X( GlobalFilterCollection filters )
        {
            filters.Add(new AuthorizeAttribute() { Roles="ADMIN"} );
        }
    }
}
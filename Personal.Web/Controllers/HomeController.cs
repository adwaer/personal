using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Personal.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return File("~/Frontend/index.html", "text/html");
        }
    }
}

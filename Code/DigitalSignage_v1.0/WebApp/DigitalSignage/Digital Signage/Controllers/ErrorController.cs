using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Digital_Signage.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View("Error");
        }

    }
}

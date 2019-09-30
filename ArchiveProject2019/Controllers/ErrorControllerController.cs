using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class ErrorControllerController : Controller
    {
        // GET: ErrorController
        public ActionResult HttpNotFoundError()
        {
            return View();
        }
        public ActionResult BadRequestError()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult AccessDenied()
        {
            return View("AccessDeniedError");
        }
       


    }
}
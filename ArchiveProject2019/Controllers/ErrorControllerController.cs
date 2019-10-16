using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;

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




        public ActionResult AccountLockout()
        {
            //Remove saving cookies:{log out from current account}
           
            IAuthenticationManager AuthenticationManager =  HttpContext.GetOwinContext().Authentication;
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            //LocoutError : view in shared views
            return View("LockOutError");
        }



    }
}
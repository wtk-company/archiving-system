using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Microsoft.AspNet.Identity.UserManager<ApplicationUser> UserManager;



       
        public HomeController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        }



       // [AccessDeniedAuthorizeattribute(ActionName = "Access")]

        public ActionResult Index()
        {
            //
            if(false)
            {
                return RedirectToAction("ApplicationClosed", "ErrorController");
            }
            return View();
        }
       


    }
}
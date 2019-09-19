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

        public ActionResult IndexTR()
        {
            return View();
        }
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Indexx()
        {
            return View();
        }


        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
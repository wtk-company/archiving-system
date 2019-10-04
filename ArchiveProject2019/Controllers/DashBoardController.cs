using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.ViewModel;

namespace ArchiveProject2019.Controllers
{
    public class DashBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DashBoard
        public ActionResult Index()
        {
            Informations info = new Informations();

            info.MainDepartmentCount = db.Departments.Where(a => a.ParentId == null).Count();
            info.AllDepartmentsCount = db.Departments.Count();
            //info.LastDateDepartmentCreate = db.Departments.OrderBy(a => a.CreatedAt).FirstOrDefault().Select(a=>a.CreatedAt);
            return View();
        }
    }
}
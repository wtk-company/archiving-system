using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class InformatiomController : Controller
    {
        // GET: Informatiom
        ApplicationDbContext db = new ApplicationDbContext();


        [AccessDeniedAuthorizeattribute(ActionName = "InformationUserGroups")]

        public ActionResult UserGroups()
        {
            ViewBag.Current = "Information";

            string CurrentUserId = this.User.Identity.GetUserId();

            IEnumerable<UserGroup> UserGroups = db.UsersGroups.Where(a => a.UserId.Equals(CurrentUserId))
                .Include(a => a.Group).Include(a => a.CreatedBy)
                .ToList();

            return View(UserGroups);
        }


        [AccessDeniedAuthorizeattribute(ActionName = "InformationUserForms")]

        public ActionResult UserForms()
        {
            ViewBag.Current = "Information";

            string CurrentUserId = this.User.Identity.GetUserId();

            List<Form> Forms = UsersDepartmentAndGroupsForms.GetUsersForms(CurrentUserId).ToList();

            return View(Forms);
        }




        [AccessDeniedAuthorizeattribute(ActionName = "InformationUserAddFavoriteForms")]

        public ActionResult Add(int? id)
        {

            ViewBag.Current = "Information";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");


            }

            return View(form);
        }


        [HttpPost]
        [ActionName("Add")]
        [AccessDeniedAuthorizeattribute(ActionName = "InformationUserAddFavoriteForms")]

        public ActionResult AddConfirm (int id)
        {
            ViewBag.Current = "Information";


            string CurrentUserId = this.User.Identity.GetUserId();
            FavouriteForms favouriteForms = new FavouriteForms {
                FormId=id,
                UserId=CurrentUserId
            };

            db.FavouriteForms.Add(favouriteForms);
            db.SaveChanges();

            return RedirectToAction("UserForms");
        }


        [AccessDeniedAuthorizeattribute(ActionName = "InformationUserDeleteFavoriteForms")]

        public ActionResult Remove(int? id)
        {
            ViewBag.Current = "Information";



            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");


            }

            return View(form);
        }


        [HttpPost]
        [ActionName("Remove")]
        [AccessDeniedAuthorizeattribute(ActionName = "InformationUserDeleteFavoriteForms")]

        public ActionResult RemoveConfirm(int id)
        {
            ViewBag.Current = "Information";


            string CurrentUserId = this.User.Identity.GetUserId();
            FavouriteForms favouriteForms = db.FavouriteForms.Where(a => a.UserId.Equals(CurrentUserId) && a.FormId == id).FirstOrDefault();
            db.FavouriteForms.Remove(favouriteForms);
            db.SaveChanges();
            return RedirectToAction("UserForms");
        }
    }
}
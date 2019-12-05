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
        public ActionResult UserGroups()
        {
            ViewBag.Current = "Information";

            string CurrentUserId = this.User.Identity.GetUserId();

            IEnumerable<UserGroup> UserGroups = db.UsersGroups.Where(a => a.UserId.Equals(CurrentUserId))
                .Include(a => a.Group).Include(a => a.CreatedBy)
                .ToList();

            return View(UserGroups);
        }


        public ActionResult UserPermissions()
        {
            ViewBag.Current = "Information";

            string CurrentUserId = this.User.Identity.GetUserId();

            string userRoleName = db.Users.Find(CurrentUserId).RoleName;
            string UserRoleId = db.Roles.Where(a => a.Name.Equals(userRoleName)).FirstOrDefault().Id;


            List<Permission> RolePermissions = db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId)).Include(a => a.Permission).Select(a => a.Permission).ToList();


            List<PermissionsUser> UserPermissionsList = new List<PermissionsUser>();
            PermissionsUser puser = null;
            foreach (Permission p in RolePermissions)
            {
                puser = new PermissionsUser()
                {
                    Permission = p,
                    Is_Active = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId)).Any(a => a.PermissionId == p.Id) ?

                   db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.PermissionId == p.Id).FirstOrDefault().Is_Active :

                   db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId) && a.PermissionId == p.Id).FirstOrDefault().Is_Active

                };
                UserPermissionsList.Add(puser);

            }

            return View(UserPermissionsList);
        }

        public ActionResult UserForms()
        {
            ViewBag.Current = "Information";

            string CurrentUserId = this.User.Identity.GetUserId();

            List<Form> Forms = UsersDepartmentAndGroupsForms.GetUsersForms(CurrentUserId).ToList();

            return View(Forms);
        }



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
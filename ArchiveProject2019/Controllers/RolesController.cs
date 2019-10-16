using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using ArchiveProject2019.HelperClasses;

namespace ArchiveProject2019.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Roles
        RoleManager<ApplicationRoles> manger;

            public RolesController()
        {

        manger = new RoleManager<ApplicationRoles>(new RoleStore<ApplicationRoles>(db));


        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesIndex")]
        public ActionResult Index(string Id="none")
        {
            
            ViewBag.Current = "Roles";

            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            IEnumerable<ApplicationRoles> roles = manger.Roles.Where(a=>!a.Name.Equals("Master")).ToList();
            List<RoleViewModel> Roles = new List<RoleViewModel>();
            RoleViewModel roleViewModel;
            foreach(ApplicationRoles R in roles)
            {
                roleViewModel = new RoleViewModel();
                roleViewModel.Id = R.Id;
                roleViewModel.Name = R.Name;
                roleViewModel.UpdatedAt = R.UpdatedAt;
                roleViewModel.CreatedAt = R.CreatedAt;
                roleViewModel.CreatedByFullName = db.Users.Find(R.CreatedById).FullName;

                Roles.Add(roleViewModel);
            }
            return View(Roles);
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesCreate")]
        public ActionResult Create()
        {
            ViewBag.Current = "Roles";

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesCreate")]
        public ActionResult Create(RoleViewModel RoleView)
        {
            ViewBag.Current = "Roles";

            if (ModelState.IsValid)
            {
                //Duplicated Role Name in DB:
                if (manger.Roles.Any(a => a.Name.Equals(RoleView.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return RedirectToAction("Index", new { Id = "CreateError" });

                }
                ApplicationRoles role = new ApplicationRoles()
                {
                    Name = RoleView.Name,
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                    CreatedById = this.User.Identity.GetUserId()

                };
                //OK:
                manger.Create(role);

                return RedirectToAction("Index", new { Id = "CreateSuccess" });


            }

            return RedirectToAction("Index");


        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesEdit")]
        public ActionResult Edit(string id)
        {
            ViewBag.Current = "Roles";

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            ApplicationRoles AppRole = manger.Roles.FirstOrDefault(a=>a.Id.Equals(id));

            if(AppRole==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            RoleViewModel RVM = new RoleViewModel()
            {
                Name = AppRole.Name
                
            };
            
            ViewBag.RoleId = AppRole.Id;

            return View(RVM);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesEdit")]
        public ActionResult Edit(RoleViewModel RoleView, string RoleId )
        {
            ViewBag.Current = "Roles";

            if (ModelState.IsValid)
            {

                ApplicationRoles AppRole = manger.Roles.FirstOrDefault(a=>a.Id.Equals( RoleId));
                if (AppRole == null)
                {
                    return RedirectToAction("HttpNotFoundError", "ErrorController");
                }
                if (manger.Roles.Where(a =>! a.Id.Equals(RoleId)).Any(a => a.Name.Equals(RoleView.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return RedirectToAction("Index", new { Id = "EditError" });

                }

                AppRole.Name = RoleView.Name;
                AppRole.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
              
                db.Entry(AppRole).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = "EditSuccess" });


            }

            return RedirectToAction("Index");



        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesDelete")]
        public ActionResult Delete(string id)
        {
            ViewBag.Current = "Roles";

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            ApplicationRoles AppRole = manger.Roles.FirstOrDefault(a => a.Id.Equals(id));

            if (AppRole == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if(CheckDelete.CheckRoleDelete(id)==false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });


            }
            RoleViewModel RVM = new RoleViewModel()
            {
                Name = AppRole.Name,
                CreatedAt=AppRole.CreatedAt,
                UpdatedAt=AppRole.UpdatedAt,
               CreatedByFullName = db.Users.Find(AppRole.CreatedById).FullName,



        };

            ViewBag.RoleId = AppRole.Id;

      



            return View(RVM);

         
        }

     
        [HttpPost,ActionName("Delete")]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "RolesDelete")]
        public ActionResult Confirm(string Id)
        {
            ViewBag.Current = "Roles";

            //Delete Permission roles:
            List<PermissionRole> PRoles = db.PermissionRoles.Where(a=>a.RoleId.Equals(Id)).ToList();

            foreach(PermissionRole PR in PRoles)
            {
                db.PermissionRoles.Remove(PR);

            }

            ApplicationRoles AppRole = manger.Roles.FirstOrDefault(a=>a.Id.Equals(Id));
            db.Roles.Remove(AppRole);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = "DeleteSuccess" });


        }


    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArchiveProject2019.Controllers
{
    public class PermissionRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

      
        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesIndex")]
        public ActionResult Index(string Id,string msg="none")
        {
            ViewBag.Current = "Roles";

            if (Id == null)
            {
                
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            RoleManager<ApplicationRoles> manger= new RoleManager<ApplicationRoles>(new RoleStore<ApplicationRoles>(db));

            ApplicationRoles role = manger.FindById(Id);
            if (role == null)
            {
                
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;

            }
            else
            {
                ViewBag.Msg = null;
            }

        


            //Role ID:
            Session["Role_Id"] = role.Id;

            //Permission Role Informations:
            var permissionRoles = db.PermissionRoles.Where(a=>a.RoleId.Equals(Id)).Include(p => p.Permission).Include(p => p.Role).Include(a=>a.CreatedBy).Include(a=>a.UpdatedBy);
            return View(permissionRoles.OrderByDescending(a=>a.CreatedAt).ToList());
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesDetails")]
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Roles";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            PermissionRole permissionRole = db.PermissionRoles.Find(id);
            if (permissionRole == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            return View(permissionRole);
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesCreate")]
        public ActionResult Create()
        {
            ViewBag.Current = "Roles";


            if (Session["Role_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            //Permission For current role:
            string r = Session["Role_Id"].ToString();
            List<int> Role_Permission = db.PermissionRoles.Where(a => a.RoleId.Equals(r)).ToList().Select(a => a.PermissionId).ToList();

            //All permission Expect current role Permission:
            List<int> NPermission = db.Permissions.Where(a=>a.TypeUser==true).Select(a => a.Id).ToList().Except(Role_Permission).ToList();
            ViewBag.RoleId = Session["Role_Id"];

            //Permissions:
            IEnumerable<Permission> Perm = db.Permissions.Where(a=>NPermission.Contains(a.Id));

            return View(Perm);
        }

     
        [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesCreate")]

        public ActionResult Create(string RoleId,List<int>Sel)
        {
            ViewBag.Current = "Roles";

            if (Sel == null)
            {
                return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(),@msg="CreateError" });

            }


            string RoleName = db.Roles.Find(RoleId).Name;

            foreach(int P_Id in Sel)
            {
                PermissionRole PR = new PermissionRole() {
                    RoleId = RoleId,
                    PermissionId = P_Id,
                    CreatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                    CreatedById=this.User.Identity.GetUserId(),
                    Is_Active=true




            };
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                string PermissionName = db.Permissions.Find(P_Id).Name;

                db.PermissionRoles.Add(PR);



                db.SaveChanges();
                string UserId = User.Identity.GetUserId();
                Notification notification = null;


                List<ApplicationUser> Users = db.Users.Where(a => !a.Id.Equals(UserId) && a.RoleName.Equals(RoleName)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم إضافة صلاحية جديدة : " + PermissionName+" للدور :"+RoleName,
                        NotificationOwnerId = UserId
                    };
                    db.Notifications.Add(notification);
                }
                db.SaveChanges();



            }

            // return View();
            return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(), @msg = "CreateSuccess" });

        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesActive")]
        public ActionResult Active(int? id)
        {
            ViewBag.Current = "Roles";

            if (Session["Role_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            PermissionRole permissionRole = db.PermissionRoles.Include(a => a.Role).Include(a=>a.CreatedBy).FirstOrDefault(a=>a.Id==id);
            if (permissionRole == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            //اختبار التفعيل من  قبل الشخص نفسه


            return View(permissionRole);
        }

        [HttpPost,ActionName("Active")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesActive")]
        public ActionResult confirm(int id)
        {
            ViewBag.Current = "Roles";
            string ActiveState = "";
            PermissionRole PermissionRole = db.PermissionRoles.Find(id);
            if(PermissionRole.Is_Active==true)
            {
                PermissionRole.Is_Active = false;
                ActiveState = "إلغاء التفعيل";
            }
            else
            {
                PermissionRole.Is_Active = true;
                ActiveState = " إعادةالتفعيل";
            }

            PermissionRole.Updatedat = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            PermissionRole.UpdatedById = this.User.Identity.GetUserId();
            db.Entry(PermissionRole).State = EntityState.Modified;



            string Role_ID = Session["Role_Id"].ToString();
            string RoleName = db.Roles.Find(Role_ID).Name;
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            string PermissionName = db.Permissions.Find(PermissionRole.PermissionId).Name;

            db.SaveChanges();

            string UserId = User.Identity.GetUserId();
            Notification notification = null;
            List<ApplicationUser> Users = db.Users.Where(a => !a.Id.Equals(UserId) && a.RoleName.Equals(RoleName)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تمت  عملية  : " + ActiveState + " للصلاحية :" + PermissionName+" للدور :"+RoleName,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();

            return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(), @msg = "ActiveSuccess" });


        }
        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Roles";

            if (Session["Role_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            PermissionRole permissionRole = db.PermissionRoles.Find(id);
            if (permissionRole == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(permissionRole);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionRolesDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Current = "Roles";

            PermissionRole permissionRole = db.PermissionRoles.Include(a => a.Role).Include(a => a.CreatedBy).FirstOrDefault(a => a.Id == id);
            db.PermissionRoles.Remove(permissionRole);
            db.SaveChanges();
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            string Role_ID = Session["Role_Id"].ToString();
            string RoleName = db.Roles.Find(Role_ID).Name;
            string UserId = User.Identity.GetUserId();
            string PermissionName = db.Permissions.Find(permissionRole.PermissionId).Name;

            Notification notification = null;
            List<ApplicationUser> Users = db.Users.Where(a => !a.Id.Equals(UserId) && a.RoleName.Equals(RoleName)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم حذف صلاحية  : " + PermissionName + " من الدور :" + RoleName,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(), @msg = "DeleteSuccess" });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArchiveProject2019.Controllers
{
    public class PermissionRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PermissionRoles
        public ActionResult Index(string Id,string msg="none")
        {
            ViewBag.Current = "Roles";

            if (Id == null)
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            RoleManager<ApplicationRoles> manger= new RoleManager<ApplicationRoles>(new RoleStore<ApplicationRoles>(db));

            ApplicationRoles role = manger.FindById(Id);
            if (role == null)
            {
                
                return HttpNotFound();

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
            var permissionRoles = db.PermissionRoles.Where(a=>a.RoleId.Equals(Id)).Include(p => p.Permission).Include(p => p.Role).Include(a=>a.CreatedBy);
            return View(permissionRoles.ToList());
        }

        // GET: PermissionRoles/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Roles";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermissionRole permissionRole = db.PermissionRoles.Find(id);
            if (permissionRole == null)
            {
                return HttpNotFound();
            }
            return View(permissionRole);
        }

        // GET: PermissionRoles/Create
        public ActionResult Create()
        {
            ViewBag.Current = "Roles";


            if (Session["Role_Id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            //Permission For current role:
            string r = Session["Role_Id"].ToString();
            List<int> Role_Permission = db.PermissionRoles.Where(a => a.RoleId.Equals(r)).ToList().Select(a => a.PermissionId).ToList();

            //All permission Expect current role Permission:
            List<int> NPermission = db.Permissions.Select(a => a.Id).ToList().Except(Role_Permission).ToList();
            ViewBag.RoleId = Session["Role_Id"];

            //Permissions:
            IEnumerable<Permission> Perm = db.Permissions.Where(a=>NPermission.Contains(a.Id));

            return View(Perm);
        }

        // POST: PermissionRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(string RoleId,List<int>Sel)
        {
            ViewBag.Current = "Roles";

            if (Sel == null)
            {
                return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(),@msg="CreateError" });

            }

            foreach(int P_Id in Sel)
            {
                PermissionRole PR = new PermissionRole() {
                    RoleId = RoleId,
                    PermissionId = P_Id,
                    CreatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                    CreatedById=this.User.Identity.GetUserId(),
                    Is_Active=true

            };
                db.PermissionRoles.Add(PR);
                db.SaveChanges();

            }

            // return View();
            return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(), @msg = "CreateSuccess" });

        }

       

        public ActionResult Active(int? id)
        {
            ViewBag.Current = "Roles";

            if (Session["Role_Id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermissionRole permissionRole = db.PermissionRoles.Include(a => a.Role).Include(a=>a.CreatedBy).FirstOrDefault(a=>a.Id==id);
            if (permissionRole == null)
            {
                return HttpNotFound();
            }
            //اختبار التفعيل من  قبل الشخص نفسه


            return View(permissionRole);
        }

        [HttpPost,ActionName("Active")]
        [ValidateAntiForgeryToken]
        
        public ActionResult confirm(int id)
        {
            ViewBag.Current = "Roles";

            PermissionRole PermissionRole = db.PermissionRoles.Find(id);
            if(PermissionRole.Is_Active==true)
            {
                PermissionRole.Is_Active = false;
            }
            else
            {
                PermissionRole.Is_Active = true;
            }

            PermissionRole.Updatedat = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            
            db.Entry(PermissionRole).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index", new { @id = Session["Role_Id"].ToString(), @msg = "ActiveSuccess" });


        }
        // GET: PermissionRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Roles";

            if (Session["Role_Id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermissionRole permissionRole = db.PermissionRoles.Find(id);
            if (permissionRole == null)
            {
                return HttpNotFound();
            }

            return View(permissionRole);
        }

        // POST: PermissionRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Current = "Roles";

            PermissionRole permissionRole = db.PermissionRoles.Include(a => a.Role).Include(a => a.CreatedBy).FirstOrDefault(a => a.Id == id);
            db.PermissionRoles.Remove(permissionRole);
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

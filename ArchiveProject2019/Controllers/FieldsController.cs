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
using ArchiveProject2019.HelperClasses;

namespace ArchiveProject2019.Controllers
{
    public class FieldsController : Controller
    {

        
        private ApplicationDbContext db = new ApplicationDbContext();



        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsIndex")]
        // GET: Fields
        public ActionResult Index(int ?Id,string msg="none")
        {
            ViewBag.Current = "Forms";

            if (Id == null)
            {
             
                return RedirectToAction("BadRequestError", "ErrorController");


            }
            //check Formis found:
            var cat = db.Forms.Find(Id);
            if (cat==null)
            {

                return RedirectToAction("HttpNotFoundError", "ErrorController");


            }

            //Check That Form for current user:
            if (!cat.CreatedById.Equals(this.User.Identity.GetUserId()))
            {
               
                return RedirectToAction("Index", new {controller="Forms"});

            }
          
            Session["Form_Id"] = Id;


            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;

            }
            else
            {
                ViewBag.Msg = null;
            }

            var fields = db.Fields.Include(a=>a.Values).Where(a=>a.FormId==Id);
            return View(fields.OrderByDescending(a =>a.CreatedAt).ToList());

        }



      


        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsDetails")]
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Forms";

            if (Session["Form_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");


            }

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            Field field = db.Fields.Include(s=>s.Form).Include(a=>a.UpdatedBy).Include(a=>a.CreatedBy).FirstOrDefault(a=>a.Id==id);
            if (field == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            return View(field);
        }

        // GET: Fields/Create


        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsCreate")]
        public ActionResult Create()
        {
            ViewBag.Current = "Forms";

            if (Session["Form_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");


            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,IsRequired,Type")] Field field)
        {
            //No session
            ViewBag.Current = "Forms";

            //Duplicated Field Name:
            int idx = Convert.ToInt32(Session["Form_Id"]);
            if (db.Fields.Where(a=>a.FormId==idx ).Any(a => a.Name.Equals(field.Name, StringComparison.OrdinalIgnoreCase)) == true)
            {

                return RedirectToAction("Index", new { Id = Convert.ToInt32(Session["Form_Id"]),msg="CreateError" });
            }


            if (ModelState.IsValid)
            {
                field.FormId = Convert.ToInt32(Session["Form_Id"]);
                field.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                field.CreatedById = User.Identity.GetUserId();

                db.Fields.Add(field);
                db.SaveChanges();
                return RedirectToAction("Index",new { Id= Convert.ToInt32(Session["Form_Id"]),msg="CreateSuccess" });
            }


                return RedirectToAction("Index");

        }

        // GET: Fields/Edit/5

        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Forms";

            if (Session["Form_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");


            }

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }


            Field field = db.Fields.Include(s => s.Form).Include(a => a.Values).Include(a => a.CreatedBy).FirstOrDefault(a => a.Id == id);
            if (field == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

         

            if (field.Values.Count()>0)
            {
                return RedirectToAction("index", new { Id = Convert.ToInt32(Session["Form_Id"]), msg = "EditCannot" });

            }


            return View(field);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsEdit")]
        public ActionResult Edit([Bind(Include = "Id,Name,IsRequired,Type,CreatedAt,CreatedById,FormId")] Field field)
        {
            ViewBag.Current = "Forms";

            int idx = Convert.ToInt32(Session["Form_Id"]);
            if (db.Fields.Where(a => a.FormId == idx && a.Id!=field.Id).Any(a => a.Name.Equals(field.Name, StringComparison.OrdinalIgnoreCase)) == true)
            {

                return RedirectToAction("Index", new { Id = Convert.ToInt32(Session["Form_Id"]), msg = "EditError" });
            }

            if (ModelState.IsValid)
            {
                field.FormId = Convert.ToInt32(Session["Form_Id"]);

                field.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                field.UpdatedById = User.Identity.GetUserId();
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { Id = Convert.ToInt32(Session["Form_Id"]),msg="EditSuccess" });
            }

            return RedirectToAction("Index");

        }

        // GET: Fields/Delete/5


        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Forms";

            if (Session["Form_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");


            }


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            Field field = db.Fields.Include(s => s.Form).Include(a=>a.Values).Include(a => a.CreatedBy).FirstOrDefault(a => a.Id == id);
            if (field == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            if (CheckDelete.checkFieldsDelete(id.Value)==false)
            {
                return RedirectToAction("index", new { Id = Convert.ToInt32(Session["Form_Id"]), msg = "DeleteError" });

            }


            return View(field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "FieldsDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Current = "Forms";

            Field field = db.Fields.Find(id);
            db.Fields.Remove(field);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = Convert.ToInt32(Session["Form_Id"]), msg = "DeleteSuccess" });
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

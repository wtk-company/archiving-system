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
            return View(fields.ToList());
        }


     
        // GET: Fields/Details/5
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
            Field field = db.Fields.Include(s=>s.Form).Include(a=>a.CreatedBy).FirstOrDefault(a=>a.Id==id);
            if (field == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            return View(field);
        }

        // GET: Fields/Create
        public ActionResult Create()
        {
            ViewBag.Current = "Forms";

            if (Session["Form_Id"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");


            }


            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // POST: Fields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { Id = Convert.ToInt32(Session["Form_Id"]),msg="EditSuccess" });
            }

            return RedirectToAction("Index");

        }

        // GET: Fields/Delete/5
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

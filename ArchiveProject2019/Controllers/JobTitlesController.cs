﻿using System;
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
    public class JobTitlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: JobTitles
        public ActionResult Index(string Id="none")
        {

            ViewBag.Current = "JobTitles";

            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            var jobTitles = db.JobTitles.Include(j => j.CreatedBy).Include(j => j.UpdatedBy);
            return View(jobTitles.ToList());
        }

        // GET: JobTitles/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "JobTitles";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobTitle = db.JobTitles.Include(a => a.CreatedBy).Include(a => a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if (jobTitle == null)
            {
                return HttpNotFound();
            }
            return View(jobTitle);
        }

        // GET: JobTitles/Create
        public ActionResult Create()
        {
            ViewBag.Current = "JobTitles";

            return View();
        }

        // POST: JobTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Symbol,MaximumMember,TypeOfDisplayForm,TypeOfDisplayDocument")] JobTitle jobTitle)
        {
            ViewBag.Current = "JobTitles";

            if (ModelState.IsValid)
            {

                //Dublicated
                if(db.JobTitles.Any(a=>a.Name.Equals(jobTitle.Name,StringComparison.OrdinalIgnoreCase)))
                {
                    return RedirectToAction("Index", new { Id = "CreateError" });


                }

                jobTitle.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    jobTitle.CreatedById = this.User.Identity.GetUserId();
                db.JobTitles.Add(jobTitle);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });
        
            }


            return View(jobTitle);
        }

        // GET: JobTitles/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "JobTitles";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobTitle = db.JobTitles.Find(id);
            if (jobTitle == null)
            {
                return HttpNotFound();
            }
            return View(jobTitle);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Symbol,MaximumMember,TypeOfDisplayForm,TypeOfDisplayDocument")] JobTitle jobTitle)
        {
            ViewBag.Current = "JobTitles";

            if (ModelState.IsValid)
            {


                if (db.Fields.Where(a => a.Id!=jobTitle.Id).Any(a => a.Name.Equals(jobTitle.Name, StringComparison.OrdinalIgnoreCase)) == true)
                {

                    return RedirectToAction("Index", new {  Id = "EditError" });
                }

              //  Numbers
                List<ApplicationUser> Users = db.Users.Include(a => a.Department).Where(a => a.JobTitleId == jobTitle.Id).ToList();
                var EG = Users.GroupBy(a => a.Department.Id);
                bool x = true;
                foreach (var v in EG)
                {
                    if (v.Count() > jobTitle.MaximumMember)
                    {
                        x = false;
                        break;
                    }
                }

                if (x == false)
                {
                    return RedirectToAction("Index", new { Id = "EditErrorMaxNumber" });

                }

                jobTitle.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                jobTitle.UpdatedById = this.User.Identity.GetUserId();
                db.Entry(jobTitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = "EditSuccess" });

            }
            return View(jobTitle);
        }

        // GET: JobTitles/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "JobTitles";


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitle jobTitle = db.JobTitles.Find(id);
            if (jobTitle == null)
            {
                return HttpNotFound();
            }


            if (CheckDelete.CheckJobTitleDelete(jobTitle.Id) == false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });


            }
            return View(jobTitle);
        }

        // POST: JobTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Current = "JobTitles";

            JobTitle jobTitle = db.JobTitles.Find(id);
            db.JobTitles.Remove(jobTitle);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = "DeleteSuccess" });

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

﻿using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class CompanyController : Controller
    {
        private ApplicationDbContext _context;

        public CompanyController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            var comp = _context.Company.Find(1);
            if (comp != null)
            {
                return View(comp);
            }
                
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(Company Company, HttpPostedFileBase file1)
        {
            if (Company == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (!CheckFileFormatting.IsImage(file1))
            {
                ModelState.AddModelError("Logo", "الملف، يجب أن يكون صورة.");
            }

            if (ModelState.IsValid)
            {
                Company.CreateById = User.Identity.GetUserId();
                Company.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                if (file1 != null)
                {
                    Company.Logo = new byte[file1.ContentLength];
                    file1.InputStream.Read(Company.Logo, 0, file1.ContentLength);
                }
                
                if (Company.Id != 0)
                {
                    _context.Entry(Company).State = EntityState.Modified;
                }
                else
                {
                    _context.Company.Add(Company);
                }

                _context.SaveChanges();
                
                return RedirectToAction("Index", "DashBoard");
            }
            
            return View(Company);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}

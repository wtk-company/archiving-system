using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class DocumentDepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? Id, string msg = "none")
        {

            ViewBag.Current = "Document";


            if (Id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Document document = db.Documents.Find(Id);
            if (document == null)
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
            Session["document_Id"] = Id;
            var DocumentDepartments = db.DocumentDepartments.Where(a => a.DocumentId == Id).Include(a => a.CreatedBy).Include(a => a.Department).Include(a => a.document).ToList();
            return View(DocumentDepartments.ToList());
        }



        public ActionResult Create(int? Id)
        {
            ViewBag.Current = "Document";


            if (Id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Document document = db.Documents.Find(Id);
            if (document == null)
            {
                return HttpNotFound();
            }


            Session["Document_Id"] = Id;


       

            List<int> Departments_Document = db.DocumentDepartments.Where(a => a.DocumentId == Id.Value).Select(a => a.DepartmentId).Distinct().ToList();

            //All Departments Expect current :
            List<int> DepartmentsId = db.Departments.Select(a => a.Id).Except(Departments_Document).ToList();
            ViewBag.DocumentIdValue = Id.Value;


            IEnumerable<DepartmentListDisplay> Departments = DepartmentListDisplay.CreateDepartmentListDisplay().Where(a => DepartmentsId.Contains(a.Id)).ToList();




            return View(Departments);

        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(int DocumentIdValue, List<int> Departments)
        {

            ViewBag.Current = "Document";


            if (Departments == null)
            {
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                DocumentDepartment documentDepartment = null;
                foreach (int i in Departments)
                {
                    documentDepartment = new DocumentDepartment()
                    {

                        DepartmentId = i,
                        DocumentId= DocumentIdValue,


                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.DocumentDepartments.Add(documentDepartment);

                    db.SaveChanges();


                }
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateSuccess" });

            }




            return RedirectToAction("Index", new { @id = DocumentIdValue});
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a => a.Department).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return HttpNotFound();
            }
            return View(documentDepartment);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentDepartment documentDepartment = db.DocumentDepartments.Find(id);
            db.DocumentDepartments.Remove(documentDepartment);
            int Form_id = documentDepartment.DocumentId;
            db.SaveChanges();
            return RedirectToAction("Index", new { @id = Form_id, @msg = "DeleteSuccess" });
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
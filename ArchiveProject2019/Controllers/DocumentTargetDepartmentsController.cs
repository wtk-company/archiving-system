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
    public class DocumentTargetDepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? Id, string msg = "none")
        {

            ViewBag.Current = "Document";

            if (Id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            Document document = db.Documents.Find(Id);
            if (document == null)
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

            Session["document_Id"] = Id;
            var DocumentTargetDepartments = db.DocumentTargetDepartments.Where(a => a.DocumentId == Id).Include(a => a.CreatedBy).Include(a => a.Department).Include(a => a.document).ToList();
            return View(DocumentTargetDepartments.ToList());
        }



        public ActionResult Create(int? Id)
        {
            ViewBag.Current = "Document";


            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");


            }

            Document document = db.Documents.Find(Id);
            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            Session["Document_Id"] = Id;


       

            List<int> Departments_Document = db.DocumentTargetDepartments.Where(a => a.DocumentId == Id.Value).Select(a => a.DepartmentId).Distinct().ToList();

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
                foreach (int i in Departments)
                {
                    var DocumentTargetDepartment = new DocumentTargetDepartment()
                    {
                        DepartmentId = i,
                        DocumentId= DocumentIdValue,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()
                    };

                    db.DocumentTargetDepartments.Add(DocumentTargetDepartment);
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
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentTargetDepartment DocumentTargetDepartment = db.DocumentTargetDepartments.Include(a => a.Department).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (DocumentTargetDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(DocumentTargetDepartment);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentTargetDepartment DocumentTargetDepartment = db.DocumentTargetDepartments.Find(id);
            db.DocumentTargetDepartments.Remove(DocumentTargetDepartment);
            int Form_id = DocumentTargetDepartment.DocumentId;
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
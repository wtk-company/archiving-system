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
    public class DocumentTargetGroupsController : Controller
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
            Session["Document_Id"] = Id;
            var DocumentTargetGroups = db.DocumentTargetGroups.Where(a => a.DocumentId == Id).Include(f => f.CreatedBy).Include(f => f.Group).Include(f => f.document);
            return View(DocumentTargetGroups.ToList());
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


           

            List<int> Group_form = db.DocumentTargetGroups.Where(a => a.DocumentId == Id.Value).Select(a => a.GroupId).Distinct().ToList();

            //All Departments Expect current :
            List<int> GroupsId = db.Groups.Select(a => a.Id).Except(Group_form).ToList();
            ViewBag.DocumentIdValue = Id.Value;


            IEnumerable<Group> Groups = db.Groups.Where(a => GroupsId.Contains(a.Id)).ToList();




            return View(Groups);

        }



       [HttpPost]
        public ActionResult Create(int DocumentIdValue, List<int> Groups)
        {

            ViewBag.Current = "Forms";

            if (Groups == null)
            {
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                foreach (int i in Groups)
                {
                    var DocumentTargetGroup = new DocumentTargetGroup()
                    {
                        GroupId = i,
                        DocumentId = DocumentIdValue,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()
                    };

                    db.DocumentTargetGroups.Add(DocumentTargetGroup);
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
            DocumentTargetGroup DocumentTargetGroup = db.DocumentTargetGroups.Include(a => a.document).Include(a=>a.CreatedBy).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (DocumentTargetGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(DocumentTargetGroup);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentTargetGroup DocumentTargetGroup = db.DocumentTargetGroups.Find(id);
            db.DocumentTargetGroups.Remove(DocumentTargetGroup);
            int Document_Id = DocumentTargetGroup.DocumentId;
            db.SaveChanges();
            return RedirectToAction("Index", new { @id = Document_Id, @msg = "DeleteSuccess" });
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
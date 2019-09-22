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
    public class DocumentGroupsController : Controller
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
            var documentGroups = db.DocumentGroups.Where(a => a.DocumentId == Id).Include(f => f.CreatedBy).Include(f => f.Group).Include(f => f.document);
            return View(documentGroups.ToList());
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


           

            List<int> Group_form = db.DocumentGroups.Where(a => a.DocumentId == Id.Value).Select(a => a.GroupId).Distinct().ToList();

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

                DocumentGroup documentGroup = null;
                foreach (int i in Groups)
                {
                    documentGroup = new DocumentGroup()
                    {

                        GroupId = i,
                        DocumentId = DocumentIdValue,
                      
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.DocumentGroups.Add(documentGroup);

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
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.document).Include(a=>a.CreatedBy).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return HttpNotFound();
            }
            return View(documentGroup);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentGroup documentGroup = db.DocumentGroups.Find(id);
            db.DocumentGroups.Remove(documentGroup);
            int Document_Id = documentGroup.DocumentId;
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
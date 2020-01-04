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
    public class DocumentPartiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentPartyIndex")]

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
            var DocumentParties = db.DocumentParties.Where(a => a.DocumentId == Id).Include(f => f.CreatedBy).Include(f => f.Party).Include(f => f.Document);
            return View(DocumentParties.OrderByDescending(a=>a.CreatedAt).ToList());
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentPartyCrete")]
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


           

            List<int> Group_form = db.DocumentParties.Where(a => a.DocumentId == Id.Value).Select(a => a.PartyId).Distinct().ToList();

            //All Departments Expect current :
            List<int> GroupsId = db.Parties.Select(a => a.Id).Except(Group_form).ToList();
            ViewBag.DocumentIdValue = Id.Value;


            IEnumerable<Party> Parties = db.Parties.Where(a => GroupsId.Contains(a.Id)).ToList();




            return View(Parties);

        }



       [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentPartyCrete")]
        public ActionResult Create(int DocumentIdValue, List<int> Parties)
        {

            ViewBag.Current = "Forms";

            if (Parties == null)
            {
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                foreach (int i in Parties)
                {
                    var DocumentParty = new DocumentParty()
                    {
                        PartyId = i,
                        DocumentId = DocumentIdValue,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()
                    };

                    db.DocumentParties.Add(DocumentParty);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateSuccess" });
            }

            return RedirectToAction("Index", new { @id = DocumentIdValue});
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentPartyDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            DocumentParty DocumentParty = db.DocumentParties.Include(a => a.Document).Include(a=>a.CreatedBy).Include(a => a.Party).SingleOrDefault(a => a.Id == id);
            if (DocumentParty == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(DocumentParty);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentPartyDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentParty DocumentParty = db.DocumentParties.Find(id);
            db.DocumentParties.Remove(DocumentParty);
            int Document_Id = DocumentParty.DocumentId;
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
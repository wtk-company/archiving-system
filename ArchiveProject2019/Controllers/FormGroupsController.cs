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
    public class FormGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index(int? Id, string msg = "none")
        {

            ViewBag.Current = "Forms";

            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");


            }

            Form form = db.Forms.Find(Id);
            if (form == null)
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
            Session["Form_Id"] = Id;
            var formGroups = db.FormGroups.Where(a => a.FormId == Id).Include(f => f.CreatedBy).Include(f => f.Group).Include(f => f.Form);
            return View(formGroups.ToList());
        }

        // GET: FormDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            FormGroup formGroup = db.FormGroups.Include(a => a.CreatedBy)
             .Include(a => a.Group).Include(a => a.Form)
                .SingleOrDefault(a => a.Id == id);
            if (formGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(formGroup);
        }

        // GET: FormDepartments/Create
        public ActionResult Create(int? Id)
        {
            ViewBag.Current = "Forms";

            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");


            }

            Form form = db.Forms.Find(Id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            Session["Form_Id"] = Id;


            int Form_Id = Convert.ToInt32(Session["Form_Id"]);

            List<int> Group_form = db.FormGroups.Where(a => a.FormId == Form_Id).Select(a => a.GroupId).Distinct().ToList();

            //All Departments Expect current :
            List<int> GroupsId = db.Groups.Select(a => a.Id).Except(Group_form).ToList();
            ViewBag.FormIdValue = Form_Id;


            IEnumerable<Group> Groups = db.Groups.Where(a => GroupsId.Contains(a.Id)).ToList();




            return View(Groups);

        }

        // POST: FormDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(int FormIdValue, List<int> Groups)
        {

            ViewBag.Current = "Forms";

            if (Groups == null)
            {
                return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                FormGroup formgroup = null;
                foreach (int i in Groups)
                {
                    formgroup = new FormGroup()
                    {

                        GroupId = i,
                        FormId = FormIdValue,
                        Is_Active = true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.FormGroups.Add(formgroup);

                    db.SaveChanges();


                }
                return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateSuccess" });

            }



            return RedirectToAction("Index");

        }

        // GET: FormDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            FormGroup formGroup = db.FormGroups.Include(a=>a.Group).Include(a=>a.Form).SingleOrDefault(a=>a.Id==id);
            if (formGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            return View(formGroup);
        }

        // POST: FormDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int Id)
        {

            FormGroup formGroup = db.FormGroups.Find(Id);
            if (formGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");



            }

            int Form_Id = formGroup.FormId;
            formGroup.Updatedat = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            if (formGroup.Is_Active == true)
            {
                formGroup.Is_Active = false;

            }
            else
            {
                formGroup.Is_Active = true;
            }
         
            db.Entry(formGroup).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { @id = Form_Id, @msg = "EditSuccess" });



        }

        // GET: FormDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            FormGroup formGroup = db.FormGroups.Include(a => a.Form).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (formGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(formGroup);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormGroup formGroup = db.FormGroups.Find(id);
            db.FormGroups.Remove(formGroup);
            int Form_id = formGroup.FormId;
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
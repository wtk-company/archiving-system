using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using ArchiveProject2019.HelperClasses;
using Microsoft.AspNet.Identity;

namespace ArchiveProject2019.Controllers
{
    public class FormDepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

    
        public ActionResult Index(int? Id,string msg="none")
        {

            ViewBag.Current = "Forms";

            if (Id == null)
            {
               
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Form form = db.Forms.Find(Id);
            if(form==null)
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
            Session["Form_Id"] = Id;
            var formDepartments = db.FormDepartments.Where(a => a.FormId == Id).Include(f => f.CreatedBy).Include(f => f.Department).Include(f => f.Form).Include(f => f.UpdatedBy);
            return View(formDepartments.ToList());
        }

        // GET: FormDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormDepartment formDepartment = db.FormDepartments.Include(a => a.CreatedBy)
                .Include(a => a.UpdatedBy).Include(a => a.Department).Include(a => a.Form)
                .SingleOrDefault(a=>a.Id==id);
            if (formDepartment == null)
            {
                return HttpNotFound();
            }
            return View(formDepartment);
        }

        // GET: FormDepartments/Create
        public ActionResult Create(int ?Id)
        {
            ViewBag.Current = "Forms";

            if (Id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Form form = db.Forms.Find(Id);
            if (form == null)
            {
                return HttpNotFound();
            }


            Session["Form_Id"] = Id;


            int Form_Id = Convert.ToInt32(Session["Form_Id"]);

            List<int> Departments_Form = db.FormDepartments.Where(a => a.FormId == Form_Id).Select(a => a.DepartmentId).Distinct().ToList();

            //All Departments Expect current :
            List<int> DepartmentsId = db.Departments.Select(a => a.Id).Except(Departments_Form).ToList();
            ViewBag.FormIdValue = Form_Id;


            IEnumerable<DepartmentListDisplay> Departments = DepartmentListDisplay.CreateDepartmentListDisplay().Where(a => DepartmentsId.Contains(a.Id)).ToList();




            return View(Departments);
          
        }

        // POST: FormDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(int FormIdValue,List<int>Departments)
        {

            ViewBag.Current = "Forms";

            if (Departments == null)
            {
                return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                FormDepartment formDepartment = null;
               foreach(int i in Departments)
                {
                    formDepartment = new FormDepartment() {

                        DepartmentId=i,
                        FormId= FormIdValue,
                        Is_Active=true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.FormDepartments.Add(formDepartment);

                db.SaveChanges();


                }
                    return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateSuccess" });
               
            }

         


            return View();
        }

        // GET: FormDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormDepartment formDepartment = db.FormDepartments.Include(a => a.Department).Include(a => a.Form).SingleOrDefault(a => a.Id == id);
            if (formDepartment == null)
            {
                return HttpNotFound();
            }


            return View(formDepartment);
        }

        // POST: FormDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int Id)
        {

            FormDepartment formDepartment = db.FormDepartments.Find(Id);
            if(formDepartment==null)
            {
                return HttpNotFound();


            }

            int Form_Id = formDepartment.FormId;
            formDepartment.Updatedat = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            if(formDepartment.Is_Active==true)
            {
                formDepartment.Is_Active = false;

            }
            else
            {
                formDepartment.Is_Active = true;
            }
            formDepartment.UpdatedById = this.User.Identity.GetUserId();
                db.Entry(formDepartment).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Index", new { @id =Form_Id , @msg = "EditSuccess" });



        }

        // GET: FormDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormDepartment formDepartment = db.FormDepartments.Include(a => a.Form).Include(a => a.Department).SingleOrDefault(a=>a.Id==id);
            if (formDepartment == null)
            {
                return HttpNotFound();
            }
            return View(formDepartment);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormDepartment formDepartment = db.FormDepartments.Find(id);
            db.FormDepartments.Remove(formDepartment);
            int Form_id = formDepartment.FormId;
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

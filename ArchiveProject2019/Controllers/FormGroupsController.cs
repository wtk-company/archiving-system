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



        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsIndex")]
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
            var formGroups = db.FormGroups.Where(a => a.FormId == Id).Include(f => f.Group).Include(f => f.Form);
            return View(formGroups.OrderByDescending(a=>a.CreatedAt).ToList());
        }

        // GET: FormDepartments/Details/5


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsDetails")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            FormGroup formGroup = db.FormGroups.Include(a => a.CreatedBy)
             .Include(a => a.Group).Include(a => a.Form).Include(a=>a.UpdatedBy)
                .SingleOrDefault(a => a.Id == id);
            if (formGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(formGroup);
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsCreate")]
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

        [HttpPost]
        //[ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsCreate")]
        public ActionResult Create(int FormIdValue, List<int> Groups)
        {

            ViewBag.Current = "Forms";

            if (Groups == null)
            {
                return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                List<string> UsersId = new List<string>();
                string NotificationTime = string.Empty;
                string UserId = User.Identity.GetUserId();
                string GroupName = string.Empty;

                Notification notification = null;
                FormGroup formgroup = null;
                foreach (int i in Groups)
                {
                    NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                    formgroup = new FormGroup()
                    {

                        GroupId = i,
                        FormId = FormIdValue,
                        Is_Active = true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.FormGroups.Add(formgroup);

                    GroupName = db.Groups.Find(formgroup.GroupId).Name;
                    List<ApplicationUser> Users = db.UsersGroups.Where(a => a.GroupId == i).Include(a=>a.User).Select(a=>a.User).ToList();
                    foreach (ApplicationUser user in Users)
                    {

                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إضافة نموذج جديد للمجموعة :"+GroupName +"، النموذج: "+ db.Forms.Find(FormIdValue).Name,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);
                    }

                }
                db.SaveChanges();
                return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateSuccess" });

            }



            return RedirectToAction("Index");

        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsEdit")]
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

        
        [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsEdit")]
        public ActionResult Edit(int Id)
        {
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            string Message = string.Empty;
        
            Notification notification = null;

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
                Message = "تم الغاء تفعيل النموذج في المجموعة :"+db.Groups.Find(formGroup.GroupId).Name;
            }
            else
            {
                formGroup.Is_Active = true;
                Message = "تم  تفعيل النموذج في المجموعة :" + db.Groups.Find(formGroup.GroupId).Name;

            }

            formGroup.UpdatedById = this.User.Identity.GetUserId();
            db.Entry(formGroup).State = EntityState.Modified;
            db.SaveChanges();

            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


            List<ApplicationUser> Users = db.UsersGroups.Where(a => a.GroupId == formGroup.GroupId).Include(a=>a.User).Select(a=>a.User).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = Message + "، النموذج :" +db.Forms.Find(Form_Id).Name ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }


            db.SaveChanges();
            return RedirectToAction("Index", new { @id = Form_Id, @msg = "EditSuccess" });



        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsDelete")]
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


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormGroupsDelete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

         //[Notification Informations]:
            //List Of users
            List<string> UsersId = new List<string>();
            //Notification date:
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            //Current user who added,deleted,updated:
            string UserId = User.Identity.GetUserId();
            //Form name:
            string FormName = string.Empty;
            //group Name:
            string GroupName = string.Empty;
            //Notification object:
            Notification notification = null;

            //[Delete object]:
            FormGroup formGroup = db.FormGroups.Find(id);

            //Form name && group name:
            FormName = db.Forms.Find(formGroup.FormId).Name;
            GroupName = db.Groups.Find(formGroup.GroupId).Name;
            //Group id for object:
            int groupId = formGroup.GroupId;

            int Form_id = formGroup.FormId;
            db.FormGroups.Remove(formGroup);

            
            db.SaveChanges();

            //[Add notifications for all student in Specefic group]:

            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            List<ApplicationUser> Users = db.UsersGroups.Where(a => a.GroupId ==groupId ).Include(a => a.User).Select(a => a.User).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم إزالة نموذج من المجموعة :" + GroupName + "، النموذج: " + FormName,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }

            db.SaveChanges();

            List<string> UsersIdForDepartment = Users.Select(a => a.Id).ToList();
            List<FavouriteForms> favoriteForms = db.FavouriteForms.Where(a => a.FormId == Form_id && UsersIdForDepartment.Contains(a.UserId)).ToList();
            foreach (FavouriteForms f in favoriteForms)
            {
                db.FavouriteForms.Remove(f);

            }
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
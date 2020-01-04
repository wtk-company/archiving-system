using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class PermissionsUsersController : Controller
    {

        public ApplicationDbContext db = new ApplicationDbContext();

        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionsUsersIndex")]
        public ActionResult Index(string Id, string msg = "none")
        {
            ViewBag.Current = "Users";

            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");

            }

            ApplicationUser user = db.Users.Find(Id);
            if (user == null)
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




            //Role ID:
            Session["User_Id"] = user.Id;
            string userRoleName = db.Users.Find(Id).RoleName;
            string UserRoleId = db.Roles.Where(a => a.Name.Equals(userRoleName)).FirstOrDefault().Id;


            List<Permission> RolePermissions = db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId)).Include(a=>a.Permission).Select(a => a.Permission).ToList();


            List<PermissionsUser> UserPermissionsList = new List<PermissionsUser>();
            PermissionsUser puser = null;
            foreach(Permission p in RolePermissions)
            {
                puser = new PermissionsUser()
                {
                    Permission = p,
                    Is_Active = db.PermissionUsers.Where(a => a.UserId.Equals(user.Id)).Any(a => a.PermissionId == p.Id) ?

                    db.PermissionUsers.Where(a => a.UserId.Equals(user.Id) && a.PermissionId == p.Id).FirstOrDefault().Is_Active :

                    db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId) && a.PermissionId == p.Id).FirstOrDefault().Is_Active
                    ,
                    CreatedAt= db.PermissionUsers.Where(a => a.UserId.Equals(user.Id)).Any(a => a.PermissionId == p.Id) ?
                       db.PermissionUsers.Where(a => a.UserId.Equals(user.Id) && a.PermissionId == p.Id).FirstOrDefault().CreatedAt
                       : ""



                };
                UserPermissionsList.Add(puser);

            }
           
            return View(UserPermissionsList.OrderByDescending(a=>a.CreatedAt).ToList());
        }
              [HttpPost]
        
        [AccessDeniedAuthorizeattribute(ActionName = "PermissionsUsersIndex")]
        public ActionResult Index(List<string>Permissions)
        {
            if(Permissions==null)
            {
                return RedirectToAction("Index",new { controller="PermissionsUsers",id= Session["User_Id"] ,msg="ActiveError"});
            }

            string NotificationTime = string.Empty;
            string NotMessage = string.Empty;

            foreach (string s in Permissions)
            {
                
                NotificationTime= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                string UserId = Session["User_Id"].ToString();

                //role id:

                string RoleName = db.Users.Find(UserId).RoleName;
                string RoleId = db.Roles.Where(a => a.Name.Equals(RoleName)).FirstOrDefault().Id;
                int Permission_Id = Convert.ToInt32(s);
                if(db.PermissionUsers.Where(a=>a.UserId.Equals(UserId)).Any(a=>a.PermissionId== Permission_Id))
                {
                    int PUserId = db.PermissionUsers.Where(a => a.UserId.Equals(UserId) && a.PermissionId == Permission_Id).FirstOrDefault().Id;
                    PermissionsUser PUser = db.PermissionUsers.Find(PUserId);
                    db.PermissionUsers.Remove(PUser);
                    NotMessage = "تم  تفعيل الصلاحية: " + db.Permissions.Find(Permission_Id).Name;

                }
                else
                {

                    if(db.PermissionRoles.Where(a=>a.RoleId.Equals(RoleId) && a.PermissionId== Permission_Id).FirstOrDefault().Is_Active==false)
                    {
                        PermissionsUser PUser = new PermissionsUser()
                        {

                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                      
                            PermissionId = Permission_Id,
                            UserId = Session["User_Id"].ToString(),
                            Is_Active = true

                        };
                        db.PermissionUsers.Add(PUser);
                        NotMessage = "تم  تفعيل الصلاحية: "+db.Permissions.Find(Permission_Id).Name;
                    }
                    else
                    {
                        PermissionsUser PUser = new PermissionsUser()
                        {

                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                         
                            PermissionId = Permission_Id,
                            UserId = Session["User_Id"].ToString(),
                            Is_Active = false

                        };
                        NotMessage = "تم  إلغاء الصلاحية: " + db.Permissions.Find(Permission_Id).Name;
                        db.PermissionUsers.Add(PUser);
                    }
                   

                }

            


                string UserNotId = User.Identity.GetUserId();
                Notification notification = null;

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = UserId,
                    Message = NotMessage,
                    NotificationOwnerId = UserNotId
                };

                db.Notifications.Add(notification);

            }

            db.SaveChanges();
            return RedirectToAction("Index", new { controller = "PermissionsUsers", id = Session["User_Id"], msg = "ActiveSuccess" });


        }



       
    }
}

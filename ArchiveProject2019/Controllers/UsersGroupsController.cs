using ArchiveProject2019.Models;
using ArchiveProject2019.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using ArchiveProject2019.HelperClasses;

namespace ArchiveProject2019.Controllers
{
    public class UsersGroupsController : Controller
    {
        private ApplicationDbContext _context;

        public UsersGroupsController()
        {
            _context = new ApplicationDbContext();
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsIndex")]
        public ActionResult Index(int? Id)
        {

            if(Id==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            var Group = _context.Groups.Find(Id);
            if(Group==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            Session["GroupId"] = Id;

            //***
            IEnumerable<string> UsersGroupFound = _context.UsersGroups.Where(a => a.GroupId == Id).Select(a=>a.UserId);

           

            var Users = _context.Users.Where(a => (!UsersGroupFound.Contains(a.Id) && !a.RoleName.Equals("Master")));
            var GroupUsers = new List<GroupUserViewModel>();

            foreach (var item in Users)
            {
                var viewModel = new GroupUserViewModel
                {
                    GroupId = Id.Value,
                    UserName = item.UserName,
                    UserId = item.Id,
                    FullName=item.FullName
                };
                GroupUsers.Add(viewModel);
            }

            return View(GroupUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsIndex")]
        public ActionResult Create(List<GroupUserViewModel> viewModel)
        {
            ViewBag.Current = "UsersGroup";

            if(viewModel==null)
            {
       return RedirectToAction("ShowUsersGroup", new { Id = Convert.ToInt32(Session["GroupId"]), msg = "CreateError" });


            }
            if (ModelState.IsValid)
            {

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                string NotificationTime = string.Empty;
                for (int i = 0; i < viewModel.Count(); i++)
                {
                    var UserGroup = new UserGroup();

                    UserGroup.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    UserGroup.CreatedById = User.Identity.GetUserId();
                    UserGroup.GroupId = viewModel[i].GroupId;
                    UserGroup.UserId = viewModel[i].UserId;

                    if (viewModel[i].IsGroupMember != true)
                    {
                        continue;    
                    }
                    

                    _context.UsersGroups.Add(UserGroup);


                   NotificationTime= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    string GroupName = _context.Groups.Find(viewModel[i].GroupId).Name;
                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = viewModel[i].UserId,
                        Message = "تم إضافتك   إلى المجموعة  :" + GroupName
                     ,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);


                }
                    _context.SaveChanges();
                    return RedirectToAction("ShowUsersGroup", new { Id = Convert.ToInt32(Session["GroupId"]), msg = "CreateSuccess" });
            }

            return RedirectToAction("Index", "Groups");
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsShowUsersGroup")]
        public ActionResult ShowUsersGroup(int? Id,string msg="none")
        {

            if(Id==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Group G = _context.Groups.Find(Id);
            if(G==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (msg != "none")
            {
                ViewBag.Msg = msg;

            }
            else
            {
                ViewBag.Msg = null;
            }
            Session["GroupId"] = Id;
            var UsersGroups = _context.UsersGroups.Include(g => g.Group).Include(a=>a.CreatedBy).Include(u => u.User).Where(u => u.GroupId == Id).ToList();

            return View(UsersGroups);
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "UserGroup";

            if (id == null || Session["GroupId"]==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var UsersGroups = _context.UsersGroups.Include(g => g.Group).Include(a => a.CreatedBy).Include(u => u.User).FirstOrDefault(a=>a.Id==id);

            if (UsersGroups == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

          

            return View(UsersGroups);
        }

        [HttpPost]
        [ActionName("Edit")]
        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsEdit")]
        public ActionResult ConfirmEdit(int?id)
        {
            ViewBag.Current = "UserGroup";

            if (id == null || Session["GroupId"] == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            UserGroup UserGroup = _context.UsersGroups.Find(id);

            if (UserGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

         
            _context.Entry(UserGroup).State = EntityState.Modified;
            _context.SaveChanges();


            return RedirectToAction("ShowUsersGroup", new { Id = Convert.ToInt32(Session["GroupId"]), msg = "EditSuccess" });


        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsEdit")]
        public ActionResult Details(int ?Id)
        {
            ViewBag.Current = "UserGroup";

            if (Id == null|| Session["GroupId"]==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var UsersGroups = _context.UsersGroups.Include(g => g.Group).Include(u => u.User).Include(a => a.CreatedBy).FirstOrDefault(a => a.Id == Id);

            if (UsersGroups == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(UsersGroups);
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsDelete")]
        public ActionResult Delete(int? id)
        {
            if(id==null|| Session["GroupId"]==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }


            var UsersGroups = _context.UsersGroups.Include(g => g.Group).Include(u => u.User).Include(a => a.CreatedBy)
                .Where(a=>a.Id==id).SingleOrDefault();

            if (UsersGroups == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(UsersGroups);
        }


        [HttpPost]
        [ActionName("Delete")]


        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersGroupsDelete")]
        public ActionResult ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }


            UserGroup UsersGroups = _context.UsersGroups.Find(id);
            if (UsersGroups == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            string UserId = User.Identity.GetUserId();
            Notification notification = null;
            string NotificationTime = string.Empty;

            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            string UserGroupId = UsersGroups.UserId;
            _context.UsersGroups.Remove(UsersGroups);
            _context.SaveChanges();
            string GroupName = _context.Groups.Find(UsersGroups.GroupId).Name;
         
            notification = new Notification()
            {

                CreatedAt = NotificationTime,
                Active = false,
                UserId = UserGroupId,
                Message = "تم إزالتك   من المجموعة  :" + GroupName
                ,
                NotificationOwnerId = UserId
            };
            _context.Notifications.Add(notification);


        
             _context.SaveChanges();

            return RedirectToAction("ShowUsersGroup", new { Id = Convert.ToInt32(Session["GroupId"]),msg="DeleteSuccess" });

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
using ArchiveProject2019.Models;
using ArchiveProject2019.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.HelperClasses;
namespace ArchiveProject2019.Controllers
{
    public class UsersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Microsoft.AspNet.Identity.UserManager<ApplicationUser> UserManager;

        //Constructor:
        public UsersController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        //Reister New User:
        
        public ActionResult UserFullName()
        {
            string uid = User.Identity.GetUserId();
            ViewBag.UserName = UserManager.Users.FirstOrDefault(a => a.Id.Equals(uid)).UserName;
            ViewBag.UserFullName = UserManager.Users.FirstOrDefault(a => a.Id.Equals(uid)).FullName;
            return PartialView("_UserName");
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersRegister")]
        public ActionResult Register()
        {
            //Role
            ViewBag.Current = "Users";
            ViewBag.Groups = new SelectList(db.Groups.ToList(), "Id", "Name");

            ViewBag.Role = new SelectList(db.Roles.Where(a => !a.Name.Equals("Master")).ToList(), "Name", "Name");


            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name");
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name");








            return View();
        }



        //Register Post:
        [HttpPost]
       

        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersRegister")]
        public async Task<ActionResult> Register(RegisterViewModel model,IEnumerable<string>Groups)

        {

            bool x = true;
            ViewBag.Current = "Users";

            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name");
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name");

            ViewBag.Role = new SelectList(db.Roles.Where(a => !a.Name.Equals("Master")).ToList(), "Name", "Name", model.Role);

            ViewBag.Groups = new SelectList(db.Groups.ToList(), "Id", "Name");


            if (ModelState.IsValid)
            {


                if (db.Users.Any(a => a.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase)))
                {


                    ModelState.AddModelError("UserName", "اسم المستخدم موجود مسبقاً يرجى اعادة الإدخال");
                    x = false;
                }

                if (CheckJobTitleDepartment.CheckJobTitleDepartmentCreateUser(model.DepartmentID, model.JobTitleId) == false)
                {

                    ModelState.AddModelError("JobTitleId", "عددالأعضاء للقسم بالنسبة للمسمى الوظيفي وصل للحد الأعظمي");
                    x = false;
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (db.Users.Any(a => a.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Email", "لا يمكن أن يكون البريد الإلكتروني مكرر، يرجى إعادةالإدخال");

                        x = false;

                    }

                }


                if (x == false)
                {
                    return View(model);


                }
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    DepartmentId = model.DepartmentID,
                    JobTitleId = model.JobTitleId,
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                    CreatedById = this.User.Identity.GetUserId(),
                    RoleName = model.Role
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);

                    
                    //Add User To Groups
                    if (Groups != null)
                    {
                        string UserId = User.Identity.GetUserId();
                        Notification notification = null;
                        string NotificationTime = string.Empty;
                        string GroupName = string.Empty;
                        foreach (string User_Group_Id in Groups)
                        {
                            var UserGroup = new UserGroup()
                            {

                                UserId = user.Id,
                                GroupId = Convert.ToInt32(User_Group_Id),
                                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                                CreatedById = this.User.Identity.GetUserId()
                            };

                            NotificationTime= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                            db.UsersGroups.Add(UserGroup);
                            GroupName = db.Groups.Find(UserGroup.GroupId).Name;
                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إضافتك   إلى المجموعة  :" + GroupName
             ,
                                NotificationOwnerId = UserId
                            };
                            db.Notifications.Add(notification);


                        }
                    }


                    db.SaveChanges();
                    return RedirectToAction("Index", new { @Id = "CreateSuccess" });
                }
                // AddErrors(result);
            }







            return View(model);
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersChangeProfile")]
        public ActionResult ChangeProfile()
        {
            ViewBag.Current = "Users";

            string Uid = this.User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(Uid);


            
            if(user==null)

            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            EditUserNameAndPassword ED = new EditUserNameAndPassword()
            {

                OldUserName = user.UserName
            };
            return View();
        }

        [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersChangeProfile")]
        public ActionResult ChangeProfile(EditUserNameAndPassword viewModel)
        {
            ViewBag.Current = "Users";

            bool status = true;
            if (ModelState.IsValid)
            {
                string Uid = this.User.Identity.GetUserId();

                ApplicationUser user = UserManager.FindById(Uid);

                if (!user.UserName.Equals(viewModel.OldUserName, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("OldUserName", " اسم المستخدم الحالية خاطئة");
                    status = false;
                }
                if (db.Users.Where(a => !a.Id.Equals(Uid)).Any(a => a.UserName.Equals(viewModel.NewUserName, StringComparison.OrdinalIgnoreCase)))
                {

                    ModelState.AddModelError("NewUserName", "يرجى تغيير اسم المستخدم لوجوده سابقا");
                    status = false;
                    //    return RedirectToAction("Index", new { @Id = "CreateErrorIdentityNumber" });

                }



                if (!UserManager.CheckPassword(user, viewModel.OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "كلمة السر الحالية خاطئة");
                    status = false;

                }

                if (status == false)
                {

                    viewModel.NewPassword = null;
                    viewModel.ConfirmPassword = null;
                    return View(viewModel);

                }
                else
                {
                    var HashPassword = UserManager.PasswordHasher.HashPassword(viewModel.NewPassword);
                    user.PasswordHash = HashPassword;
                    user.UserName = viewModel.NewUserName;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "DashBoard");
                }

            }

            return View(viewModel);
        }




        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersIndex")]
        public ActionResult Index(string Id="none")

        {
            ViewBag.Current = "Users";


            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            string Uid = this.User.Identity.GetUserId();
            IEnumerable<ApplicationUser> Users = UserManager.Users.OrderByDescending(a=>a.CreatedAt).ToList();
            return View(Users);
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersDetails")]

        public ActionResult Details(string id)
        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            ApplicationUser User = UserManager.FindById(id);
            if (User == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            var UserModel = new UserViewModel()
            {
                FullName = User.FullName,
                Gender = User.Gender,
                CreatedAt = User.CreatedAt,
                UpdatedAt = User.UpdatedAt,
                UserName = User.UserName,
                DepartmentName = User.DepartmentId == null ? "" : db.Departments.Find(User.DepartmentId).Name,
                JobTitle = User.JobTitleId == null ? "" : db.JobTitles.Find(User.JobTitleId).Name,
                CreatedBy = string.IsNullOrEmpty(User.CreatedById) ? "" : UserManager.FindById(User.CreatedById).FullName,
                RoleName = User.RoleName,
                Email = User.Email,
              UpdatedBy= User.UpdatedByID==null?"": db.Users.Find(User.UpdatedByID).FullName
            };

            return View(UserModel);
        }




        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersEdit")]

        public ActionResult Edit(string id)
        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }


            if(CheckDelete.CheckUserEdit(id)==false)
            {
                
                    return RedirectToAction("Index", new { Id = "OperationError" });

            }
            EditProfileViewModel EProfile = new EditProfileViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.RoleName,
                Id = user.Id,
                UserName=user.UserName,
                Gender = user.Gender,
                JobTitleId = user.JobTitleId == null ? 0 : user.JobTitleId.Value,
                DepartmentID = user.DepartmentId == null ? 0 : user.DepartmentId.Value


            };

            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name", EProfile.Role);

            List<int> SelectedGroups = new List<int>();
            SelectedGroups = db.UsersGroups.Where(a => a.UserId.Equals(id)).Select(a => a.GroupId).ToList();

            List<SelectListItem> ListSl = new List<SelectListItem>();
            foreach (var G in db.Groups.ToList())
            {
                var sl = new SelectListItem()
                {

                    Text = G.Name,
                    Value = G.Id.ToString(),
                    Selected = SelectedGroups.DefaultIfEmpty().Contains(G.Id) ? true : false
                };

                ListSl.Add(sl);

            }
            ViewBag.Groups = ListSl;


            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name", EProfile.DepartmentID);
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name", EProfile.JobTitleId);


            return View(EProfile);
        }

      
        [HttpPost]


        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersEdit")]
        public ActionResult Edit( EditProfileViewModel EProfile, IEnumerable<string> Groups)

        {
            ViewBag.Current = "Users";
            string OldUserRole = db.Users.Find(EProfile.Id).RoleName;
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Id", "Name", EProfile.Role);

            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name", EProfile.DepartmentID);
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name", EProfile.JobTitleId);

            List<int> SelectedGroups = new List<int>();
            SelectedGroups = db.UsersGroups.Where(a => a.UserId.Equals(EProfile.Id)).Select(a => a.GroupId).ToList();
            SelectListItem sl;
            List<SelectListItem> ListSl = new List<SelectListItem>();
            foreach (var G in db.Groups.ToList())
            {
                sl = new SelectListItem()
                {

                    Text = G.Name,
                    Value = G.Id.ToString(),
                    Selected = SelectedGroups.DefaultIfEmpty().Contains(G.Id) ? true : false
                };

                ListSl.Add(sl);

            }
            ViewBag.Groups = ListSl;
            bool x = true;
            ApplicationUser user = null;
            if (ModelState.IsValid)
            {


                user = UserManager.FindById(EProfile.Id);
                if (user == null)
                {
                    return RedirectToAction("HttpNotFoundError", "ErrorController");

                }

                //Delete All Users Permissions:
                if(!user.RoleName.Equals(EProfile.Role))
                {



                    List<PermissionsUser> User_Permissions = db.PermissionUsers.Where(a => a.UserId.Equals(user.Id)).ToList();
                    foreach(PermissionsUser UP in User_Permissions)
                    {
                        db.PermissionUsers.Remove(UP);
                    }
                    db.SaveChanges();
                }



                if (!EProfile.Role.Equals("Master"))
                {
                    if (CheckJobTitleDepartment.CheckJobTitleDepartmentCreateUser(EProfile.DepartmentID, EProfile.JobTitleId, EProfile.Id) == false)
                    {

                        ModelState.AddModelError("JobTitleId", "عددالأعضاء للقسم بالنسبة للمسمى الوظيفي وصل للحد الأعظمي");
                        x = false;
                    }
                }



                if (db.Users.Where(a => !a.Id.Equals(EProfile.Id)).Any(a => a.UserName.Equals(EProfile.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("UserName", "لا يمكن أن يكون اسم امستخدم  مكرر، يرجى إعادةالإدخال");

                    x = false;

                }

                if (!string.IsNullOrEmpty(EProfile.Email))
                {
                    if (db.Users.Where(a => !a.Id.Equals(EProfile.Id)).Any(a => a.Email.Equals(EProfile.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Email", "لا يمكن أن يكون البريد الإلكتروني مكرر، يرجى إعادةالإدخال");

                        x = false;

                    }

                }

                if (x == false)
                {
                    return View(EProfile);
                }
                user.FullName = EProfile.FullName;
                user.Email = EProfile.Email;
                user.Gender = EProfile.Gender;
                user.RoleName = EProfile.Role;
                user.UserName = EProfile.UserName;
                var HashPassword = UserManager.PasswordHasher.HashPassword(EProfile.Password);
                user.PasswordHash = HashPassword;

                user.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
               user.UpdatedByID = this.User.Identity.GetUserId();
                
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                //Add User To Groups

                //

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                string NotificationTime = string.Empty;
                string GroupName = string.Empty;
                NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم تحديث معلوماتك الشخصية",
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);


            
            List<string> SelectedUserGroups = new List<string>();
                SelectedUserGroups = db.UsersGroups.Where(a => a.UserId.Equals(EProfile.Id)).Select(a => a.GroupId.ToString()).ToList();
                if (Groups != null)
                {
                    UserGroup UserGroup = null;
                    List<string> ExpectGroups = new List<string>();
                    ExpectGroups = SelectedUserGroups.Except(Groups).ToList();
                    foreach (string User_Group_Id in Groups)
                    {

                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                        if (SelectedUserGroups.Contains(User_Group_Id))
                        {

                            continue;
                        }
                        UserGroup = new UserGroup()
                        {

                            UserId = user.Id,
                            GroupId = Convert.ToInt32(User_Group_Id),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        db.UsersGroups.Add(UserGroup);
                        GroupName = db.Groups.Find(UserGroup.GroupId).Name;
                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إضافتك   إلى المجموعة  :" + GroupName
          ,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);


                    }


                    UserGroup deleteUserGroup;
                    foreach (string s in ExpectGroups)
                    {
                        deleteUserGroup = db.UsersGroups.Where(a => a.UserId.Equals(EProfile.Id) && a.GroupId.ToString().Equals(s)).SingleOrDefault();

                        db.UsersGroups.Remove(deleteUserGroup);
                        GroupName = db.Groups.Find(deleteUserGroup.GroupId).Name;
                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إزالتك   من المجموعة  :" + GroupName
          ,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);

                    }
                    db.SaveChanges();

                }

                else
                {
                    foreach (UserGroup ug in db.UsersGroups.Where(a => a.UserId.Equals(EProfile.Id)))
                    {
                        db.UsersGroups.Remove(ug);
                        GroupName = db.Groups.Find(ug.GroupId).Name;
                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إزالتك   من المجموعة  :" + GroupName
          ,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);
                    }

                    db.SaveChanges();

                }

                db.SaveChanges();



                return RedirectToAction("Index", new { @Id = "EditSuccess" });

            }













            return View(EProfile);
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersDelete")]
        public ActionResult Delete(string  id)

        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            if (CheckDelete.CheckUserDelete(id) == false)
            {

                return RedirectToAction("Index", new { Id = "OperationError" });

            }
            return View(user);
        }


     
        [HttpPost,ActionName("Delete")]

        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersDelete")]

        public ActionResult confirm(string Id)
        {
            ViewBag.Current = "Users";

            ApplicationUser user = UserManager.FindById(Id);
            if (user == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }



            List<FavouriteForms> UserFavoriteForm = db.FavouriteForms.Where(a => a.UserId.Equals(Id)).ToList();
            foreach (var v in UserFavoriteForm)
            {
                db.FavouriteForms.Remove(v);

            }
            db.SaveChanges();

            List<UserGroup> UserGroup = db.UsersGroups.Where(a => a.UserId.Equals(Id)).ToList();
            foreach (var v in UserGroup)
            {
                db.UsersGroups.Remove(v);
            }
            db.SaveChanges();



            //Permissions:

            List<PermissionsUser> PermissionUser = db.PermissionUsers.Where(a => a.UserId.Equals(Id)).ToList();
            foreach (var v in PermissionUser)
            {
                db.PermissionUsers.Remove(v);
            }
            db.SaveChanges();


            //Notifications:

            List<Notification> UserNotifications = db.Notifications.Where(a => a.UserId.Equals(Id)).ToList();
            foreach (var v in UserNotifications)
            {
                db.Notifications.Remove(v);
            }
            db.SaveChanges();


            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index", new { @Id = "DeleteSuccess" });

        }





        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersLockOut")]
        public ActionResult LockOut(string id)
        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            if (CheckDelete.CheckUserLockOut(id) == false)
            {

                return RedirectToAction("Index", new { Id = "OperationError" });

            }

            if (user.LockoutEnabled==true)
            {
                ViewBag.LockState = "A";
            }
            else
            {
                ViewBag.LockState = "B";

            }


            return View(user);
        }




        [HttpPost]
        [ActionName("LockOut")]
        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersLockOut")]

        public ActionResult ConfirmLockOut(string Id)
        {
          

            ViewBag.Current = "Users";

            ApplicationUser user = UserManager.FindById(Id);
            if (user == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            if (user.LockoutEnabled==true)
            {
                user.LockoutEnabled = false;
            }
            else
            {
                user.LockoutEnabled = true;
            }

            user.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
           // user.UpdatedById = this.User.Identity.GetUserId();
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { @Id = "LockSuccess" });
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersRegisterMasterUser")]
        public ActionResult RegisterMasterUser()
        {
            //Role


            ViewBag.Role = "Master";









            return View();
        }



        [HttpPost]
        [AllowAnonymous]

        
        [AccessDeniedAuthorizeattribute(ActionName = "UsersRegisterMasterUser")]
        public async Task<ActionResult> RegisterMasterUser(RegisterViewModel model)
        {

            bool x = true;
            ViewBag.Current = "Users";

            ViewBag.Role = "Master";


            if (ModelState.IsValid)
            {


                if (db.Users.Any(a => a.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase)))
                {


                    ModelState.AddModelError("UserName", "اسم المستخدم موجود مسبقاً يرجى اعادة الإدخال");
                    x = false;
                }


                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (db.Users.Any(a => a.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Email", "لا يمكن أن يكون البريد الإلكتروني مكرر، يرجى إعادةالإدخال");

                        x = false;

                    }

                }


                if (x == false)
                {
                    return View(model);


                }
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                 
                    IsDefaultMaster=false,
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                    CreatedById = this.User.Identity.GetUserId(),
                    RoleName = model.Role
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);




                    db.SaveChanges();
                    return RedirectToAction("Index", new { @Id = "CreateSuccess" });
                }
                // AddErrors(result);
            }







            return View(model);
        }



    }
}

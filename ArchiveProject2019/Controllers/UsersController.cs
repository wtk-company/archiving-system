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
            string uid = this.User.Identity.GetUserId();
            ViewBag.UserName = UserManager.Users.FirstOrDefault(a => a.Id.Equals(uid)).UserName;
            ViewBag.UserFullName = UserManager.Users.FirstOrDefault(a => a.Id.Equals(uid)).FullName;
            return PartialView("_UserName");
        }
        public ActionResult Register()
        {
            //Role
            ViewBag.Current = "Users";

            ViewBag.Role = new SelectList(db.Roles.Where(a=>!a.Name.Equals("Master")).ToList(), "Name","Name");


            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(),"Id","Name");
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name");



            ViewBag.Groups = db.Groups.ToList();




           
            return View();
        }


      
        //Register Post:
        [HttpPost]
        [AllowAnonymous]
       //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model,IEnumerable<string>Groups)
        {

            bool x = true;
            ViewBag.Current = "Users";

            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name");
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name");

            ViewBag.Role = new SelectList(db.Roles.Where(a => !a.Name.Equals("Master")).ToList(), "Name", "Name",model.Role);
            ViewBag.Groups = db.Groups.ToList();


            if (ModelState.IsValid)
            {

                
                if (db.Users.Any(a=>a.UserName.Equals(model.UserName,StringComparison.OrdinalIgnoreCase)))
                {


                    ModelState.AddModelError("UserName", "اسم المستخدم موجود مسبقاً يرجى اعادة الإدخال");
                    x = false;
                }
               
                if(CheckJobTitleDepartment.CheckJobTitleDepartmentCreateUser(model.DepartmentID,model.JobTitleId)==false)
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


                if (x==false)
                {
                    return View(model);


                }
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    DepartmentId=model.DepartmentID,
                    JobTitleId=model.JobTitleId,
                   
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                    CreatedById = this.User.Identity.GetUserId(),
                    RoleName=model.Role
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);

                 


                    //Add User To Groups
                    if(Groups!=null)
                    {
                    UserGroup UserGroup=null;

                        foreach(string User_Group_Id in Groups)
                        {
                            UserGroup = new UserGroup()
                            {

                                UserId = user.Id,
                                GroupId = Convert.ToInt32(User_Group_Id),
                                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                                CreatedById = this.User.Identity.GetUserId()
                            };

                            db.UsersGroups.Add(UserGroup);
                        }

                    }

                    
                    db.SaveChanges();
                    return RedirectToAction("Index",new { @Id="CreateSuccess"});
                }
               // AddErrors(result);
            }





            //GROUPS aND Departments:

            ViewBag.Groups = db.Groups.ToList();

      

            // If we got this far, something failed, redisplay form

            return View(model);
        }


        public ActionResult ChangeProfile()
        {
            ViewBag.Current = "Users";

            string Uid = this.User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(Uid);
            if(user==null)
            {
                return HttpNotFound();

            }
            EditUserNameAndPassword ED = new EditUserNameAndPassword() {

                OldUserName=user.UserName
            };
            return View();
        }

        [HttpPost]
        public ActionResult ChangeProfile(EditUserNameAndPassword viewModel)
        {
            ViewBag.Current = "Users";

            bool status = true;
            if(ModelState.IsValid)
            {
                string Uid = this.User.Identity.GetUserId();

                ApplicationUser user = UserManager.FindById(Uid);

                if(!user.UserName.Equals(viewModel.NewUserName,StringComparison.OrdinalIgnoreCase))
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



                if (!UserManager.CheckPassword(user,viewModel.OldPassword))
                {
                    ModelState.AddModelError("OldPassword","كلمة السر الحالية خاطئة");
                    status = false;

                }

                if(status==false)
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
                    return RedirectToAction("Index","Home");
                }

            }

            return View(viewModel);
        }
        // GET: Users
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
            IEnumerable<ApplicationUser> Users = UserManager.Users.ToList();
           return View(Users);
        }
        
        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return new  HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            ApplicationUser User = UserManager.FindById(id);
            if(User==null)
            {
                return HttpNotFound();

            }

            UserViewModel UserModel;
            UserModel= new UserViewModel() {

                FullName = User.FullName,
                Gender = User.Gender,
                CreatedAt = User.CreatedAt,
                UpdatedAt = User.UpdatedAt,
                UserName = User.UserName,
                DepartmentName =User.DepartmentId==null?"": db.Departments.Find(User.DepartmentId).Name,
                JobTitle =User.JobTitleId==null?"": db.JobTitles.Find(User.JobTitleId).Name,
                CreatedBy = string.IsNullOrEmpty(User.CreatedById) ? "" : UserManager.FindById(User.CreatedById).FullName,
            //    UpdatedBy = string.IsNullOrEmpty(User.UpdatedById) ? "" : UserManager.FindById(User.UpdatedById).FullName,
                RoleName=User.RoleName,
                Email=User.Email
            };

            return View(UserModel);
        }



       


        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            ApplicationUser user = UserManager.FindById(id);
            if (user==null)
            {
                return HttpNotFound();

            }

            EditProfileViewModel EProfile = new EditProfileViewModel() {

                Email=user.Email,
                FullName=user.FullName,
                Role=user.RoleName,
               // DepartmentId=user.DepartmentId,
              
     
                Id=user.Id,
                Gender=user.Gender,
                JobTitleId=user.JobTitleId.Value,
                DepartmentID=user.DepartmentId.Value

            };
            
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name",EProfile.Role);

            ViewBag.Groups = GroupSelected.UserGroups(id);
            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name",EProfile.DepartmentID);
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name",EProfile.JobTitleId);


            return View(EProfile);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit( EditProfileViewModel EProfile, IEnumerable<string> Groups)
        {
            ViewBag.Current = "Users";
            bool x = true;
            ApplicationUser user = null;
            if (ModelState.IsValid)
            {


                 user = UserManager.FindById(EProfile.Id);
                if(user==null)
                {
                    return HttpNotFound();

                }



                

                if (CheckJobTitleDepartment.CheckJobTitleDepartmentCreateUser(EProfile.DepartmentID, EProfile.JobTitleId,EProfile.Id) == false)
                {

                    ModelState.AddModelError("JobTitleId", "عددالأعضاء للقسم بالنسبة للمسمى الوظيفي وصل للحد الأعظمي");
                    x = false;
                }

                if (!string.IsNullOrEmpty(EProfile.Email))
                {
                    if (db.Users.Where(a=>!a.Id.Equals(EProfile.Id)).Any(a => a.Email.Equals(EProfile.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("Email", "لا يمكن أن يكون البريد الإلكتروني مكرر، يرجى إعادةالإدخال");

                        x = false;

                    }

                }

                user.FullName = EProfile.FullName;
                user.Email = EProfile.Email;
                user.Gender = EProfile.Gender;     
                user.RoleName = EProfile.Role;
                user.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                user.UpdatedById = this.User.Identity.GetUserId();
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();



                return RedirectToAction("Index", new { @Id = "EditSuccess" });

            }





            ViewBag.Groups = GroupSelected.UserGroups(EProfile.Id);

       





            ViewBag.Role = new SelectList(db.Roles.ToList(), "Id", "Name", EProfile.Role);

            ViewBag.DepartmentID = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name", EProfile.DepartmentID);
            ViewBag.JobTitleId = new SelectList(db.JobTitles.ToList(), "Id", "Name", EProfile.JobTitleId);


            return View(EProfile);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string  id)
        {
            ViewBag.Current = "Users";

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            ApplicationUser user = UserManager.FindById(id);
            if(user==null)
            {
                return HttpNotFound();

            }


            //if(!CheckUsersDelete.UserCanDelete(id))
            //{
            //    return RedirectToAction("Index", new { @Id = "DeleteError" });

            //}
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult confirm(string Id)
        {
            ViewBag.Current = "Users";

            ApplicationUser user = UserManager.FindById(Id);
            if (user == null)
            {
                return HttpNotFound();

            }

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index", new { @Id = "DeleteSuccess" });

        }
    }
}

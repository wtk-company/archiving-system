using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace ArchiveProject2019.HelperClasses
{
    public class AccessDeniedAuthorizeattribute:AuthorizeAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string ActionName { set; get; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            //Non Authorize:
            base.OnAuthorization(filterContext);
<<<<<<< HEAD



            if (false)
=======
            
            if(filterContext.Result is HttpUnauthorizedResult)
>>>>>>> raeed-3-1-2020
            {

               filterContext.Result = new RedirectResult("~/ErrorController/ApplicationClosed");
                return;


            }
          

<<<<<<< HEAD
            //if (!ActionName.Equals("Access"))
            //{





                if (filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.Result = new RedirectResult("~/ErrorController/NonAuthorize");
                }

                else
                {



                    string CurrentUserId = HttpContext.Current.User.Identity.GetUserId();
                    string userRoleName = db.Users.Find(CurrentUserId).RoleName;
                    bool IsMasterD = false;
                    IsMasterD = db.Users.Find(CurrentUserId).IsDefaultMaster;
                    string UserRoleId = db.Roles.Where(a => a.Name.Equals(userRoleName)).FirstOrDefault().Id;


                    if (ActionName.Equals("DashBoard"))
=======
            string CurrentUserId = HttpContext.Current.User.Identity.GetUserId();
            var CurrentUser = db.Users.Find(CurrentUserId);

            if (CurrentUser != null)
            {
                var userRoleName = CurrentUser.RoleName;
                bool IsMasterD = false;
                IsMasterD = db.Users.Find(CurrentUserId).IsDefaultMaster;
                string UserRoleId = db.Roles.Where(a => a.Name.Equals(userRoleName)).FirstOrDefault().Id;


                if (ActionName.Equals("DashBoard"))
                {
                    ApplicationUser user = db.Users.Find(CurrentUserId);
                    if (user.LockoutEnabled == true)
                    {
                        filterContext.Result = new RedirectResult("~/ErrorController/AccountLockout");
                    }
                }
                else
                {
                    //Lock Account
                    ApplicationUser user = db.Users.Find(CurrentUserId);
                    if (user.LockoutEnabled == true)
>>>>>>> raeed-3-1-2020
                    {
                        ApplicationUser user = db.Users.Find(CurrentUserId);
                        if (user.LockoutEnabled == true)
                        {

<<<<<<< HEAD
                            filterContext.Result = new RedirectResult("~/ErrorController/AccountLockout");


                        }

                        //     return;

=======
                        filterContext.Result = new RedirectResult("~/ErrorController/AccountLockout");
>>>>>>> raeed-3-1-2020
                    }

                    else
                    {
                        List<int> PermissionRoleIs_Active = db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId) && a.Is_Active == true).Select(a => a.PermissionId).ToList();
                        List<int> NonIs_ActiveUserPermissions = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.Is_Active == false).Select(a => a.PermissionId).ToList();
                        List<int> Is_ActiveUserPermissions = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.Is_Active == true).Select(a => a.PermissionId).ToList();

<<<<<<< HEAD


=======
                        List<int> UserPermissons = PermissionRoleIs_Active.Except(NonIs_ActiveUserPermissions).ToList();
                        UserPermissons = UserPermissons.Union(Is_ActiveUserPermissions).ToList();
                        List<string> PermissionsAction = db.Permissions.Where(a => UserPermissons.Contains(a.Id)).Select(a => a.Action).ToList();
>>>>>>> raeed-3-1-2020

                        //Lock Account
                        ApplicationUser user = db.Users.Find(CurrentUserId);
                        if (user.LockoutEnabled == true)
                        {

                            filterContext.Result = new RedirectResult("~/ErrorController/AccountLockout");


                        }
                        else
                        {


                            List<int> PermissionRoleActive = db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId) && a.Is_Active == true).Select(a => a.PermissionId).ToList();
                            List<int> NonActiveUserPermissions = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.Is_Active == false).Select(a => a.PermissionId).ToList();
                            List<int> ActiveUserPermissions = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.Is_Active == true).Select(a => a.PermissionId).ToList();

                            List<int> UserPermissons = PermissionRoleActive.Except(NonActiveUserPermissions).ToList();
                            UserPermissons = UserPermissons.Union(ActiveUserPermissions).ToList();
                            List<string> PermissionsAction = db.Permissions.Where(a => UserPermissons.Contains(a.Id)).Select(a => a.Action).ToList();

                            if (!PermissionsAction.Contains(ActionName))
                            {

                                filterContext.Result = new RedirectResult("~/ErrorController/AccessDenied");
                            }

                        }
                    }
<<<<<<< HEAD






                }


           // }

          
=======
                }
            }
>>>>>>> raeed-3-1-2020
        }
    }
}
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
            base.OnAuthorization(filterContext);
            
            if(filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/ErrorController/AccessDenied");
            }

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
                    {

                        filterContext.Result = new RedirectResult("~/ErrorController/AccountLockout");
                    }
                    else
                    {
                        List<int> PermissionRoleIs_Active = db.PermissionRoles.Where(a => a.RoleId.Equals(UserRoleId) && a.Is_Active == true).Select(a => a.PermissionId).ToList();
                        List<int> NonIs_ActiveUserPermissions = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.Is_Active == false).Select(a => a.PermissionId).ToList();
                        List<int> Is_ActiveUserPermissions = db.PermissionUsers.Where(a => a.UserId.Equals(CurrentUserId) && a.Is_Active == true).Select(a => a.PermissionId).ToList();

                        List<int> UserPermissons = PermissionRoleIs_Active.Except(NonIs_ActiveUserPermissions).ToList();
                        UserPermissons = UserPermissons.Union(Is_ActiveUserPermissions).ToList();
                        List<string> PermissionsAction = db.Permissions.Where(a => UserPermissons.Contains(a.Id)).Select(a => a.Action).ToList();

                        if (!PermissionsAction.Contains(ActionName))
                        {

                            filterContext.Result = new RedirectResult("~/ErrorController/AccessDenied");
                        }
                    }
                }
            }
        }
    }
}
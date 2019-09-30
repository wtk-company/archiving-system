using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.HelperClasses
{
    public class AccessDeniedAuthorizeattribute:AuthorizeAttribute
    {
        public string ActionName { set; get; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if(filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/ErrorController/AccessDenied");
            }

            if(ActionName.Equals("Asmi"))
            {
                filterContext.Result = new RedirectResult("~/ErrorController/AccessDenied");

            }
        }

    }
}
using KodlaTv.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KodlaTv.WebApp.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User != null && CurrentSession.User.isAdmin == false)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}
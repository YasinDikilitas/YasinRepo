using KodlaTv.BusinessLayer;
using KodlaTv.Common;
using KodlaTv.WebApp.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KodlaTv.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private CategoryManager categorymanager = new CategoryManager();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            App.Common = new WebCommon();
        }
    }
}

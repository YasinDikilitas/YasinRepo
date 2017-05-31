using KodlaTv.Common;
using KodlaTv.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KodlaTv.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                KodlatvUser user = HttpContext.Current.Session["login"] as KodlatvUser;
                return user.Username;

            }
            return "system";
        }
    }
}
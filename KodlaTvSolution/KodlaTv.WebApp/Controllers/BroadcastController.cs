using KodlaTv.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KodlaTv.WebApp.Controllers
{
    [Auth]
    [Exc]
    public class BroadcastController : Controller
    {
        // GET: Broadcast
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OBS()
        {
            return View();
        }
    }
}
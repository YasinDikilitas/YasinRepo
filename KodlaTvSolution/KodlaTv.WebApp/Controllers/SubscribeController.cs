using KodlaTv.BusinessLayer;
using KodlaTv.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KodlaTv.WebApp.Controllers
{
    [Exc]
    public class SubscribeController : Controller
    {
        SubscribeManager subscribemanager = new SubscribeManager();
        // GET: Subscribe
        public ActionResult Index()
        {
            return View();
        }
        [Auth]
        public ActionResult MySubscribe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(subscribemanager.List(x => x.Owner.id == id).OrderByDescending(x => x.CreatedOn));
        }

        public ActionResult OtherSubscribe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(subscribemanager.List(x => x.Channel.id == id).OrderByDescending(x => x.CreatedOn));
        }
    }
}
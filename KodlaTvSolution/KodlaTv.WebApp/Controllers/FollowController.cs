using KodlaTv.BusinessLayer;
using KodlaTv.Entities;
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
    public class FollowController : Controller
    {
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        private FollowManager followmanager = new FollowManager();
        // GET: Follow
        public ActionResult Index()
        {
            return View();
        }
        [Auth]
        public ActionResult MyFollow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(followmanager.List(x=>x.Owner.id==id).OrderByDescending(x=>x.CreatedOn));
        }

        public ActionResult OtherFollow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(followmanager.List(x => x.Channel.id == id).OrderByDescending(x => x.CreatedOn));
        }

    }
}
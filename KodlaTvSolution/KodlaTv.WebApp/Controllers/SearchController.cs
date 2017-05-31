using KodlaTv.BusinessLayer;
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
    public class SearchController : Controller
    {
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchChannel()
        {
            return View();
        }
        public ActionResult SearchVideo()
        {
            ViewBag.AllCategoryid = ViewBag.Categoryid = new SelectList(categorymanager.List(), "id", "Coursetitle");
            return View();
        }

        [HttpPost]
        public ActionResult SearchVideo(string searchword, string allCategoryid)
        {
            ViewBag.AllCategoryid = ViewBag.Categoryid = new SelectList(categorymanager.List(), "id", "Coursetitle");
            var videolist = videomanager.List();
            if (!String.IsNullOrEmpty(searchword))
            {
                videolist = videolist.Where(x => x.Videoinfo.Contains(searchword)).OrderByDescending(x => x.CreatedOn).ToList();
            }

            if (!String.IsNullOrEmpty(allCategoryid))
            {
                int gr = Convert.ToInt32(allCategoryid);
                return View(videolist.Where(x=>x.Category.id==gr).OrderByDescending(x => x.CreatedOn).ToList());
            }
            else
            {
                return View(videolist);
            }
        }

        [HttpPost]
        public ActionResult SearchChannel(string searchword)
        {
            var channellist = channelmanager.List();
            if (!String.IsNullOrEmpty(searchword))
            {
                channellist = channellist.Where(x => x.ChannelName.Contains(searchword)).OrderByDescending(x => x.Follows.Count).ToList();
            }
            return View(channellist);
        }

        [HttpPost]
        public ActionResult Index(string searchword)
        {
            var userlist = kodlatvusermanager.List();
            if (!String.IsNullOrEmpty(searchword))
            {
                userlist = userlist.Where(x => x.Username.Contains(searchword)|| x.Name.Contains(searchword)|| x.Surname.Contains(searchword)).OrderByDescending(x => x.Follows.Count).ToList();
            }
            return View(userlist);
        }

    }
}
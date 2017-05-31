using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KodlaTv.Entities;
using KodlaTv.WebApp.Models;
using KodlaTv.BusinessLayer;
using KodlaTv.Entities.Messages;
using KodlaTv.WebApp.ViewModels;
using KodlaTv.WebApp.Filters;

namespace KodlaTv.WebApp.Controllers
{
    [Exc]
    public class VideoController : Controller
    {
        private VideoManager videomanager = new VideoManager();
        private CategoryManager categorymanager = new CategoryManager();
        private ChannelManager channelmanager = new ChannelManager();
        // GET: Video
        public ActionResult Index()
        {
            return View(videomanager.List());
        }
        [Auth]
        public ActionResult MyVideo(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(videomanager.ListQueryable().Where(x=>x.Channel.Owner.id==id).OrderByDescending(x=>x.CreatedOn));
        }

        public ActionResult Watchvideo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = videomanager.Find(x => x.id == id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }
       
        [Auth]
        [AuthAdmin]
        // GET: Video/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = videomanager.Find(x => x.id == id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);

        }
        
        [Auth]
        [AuthAdmin]
        // GET: Video/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Kolay", Value = "Kolay", Selected = true });

            items.Add(new SelectListItem { Text = "Orta", Value = "Orta" });

            items.Add(new SelectListItem { Text = "İleri", Value = "İleri" });

            ViewBag.Levelofvideo = items;
            ViewBag.Categoryid = new SelectList(categorymanager.List(), "id", "Coursetitle");

            return View();
        }
       
        // POST: Video/Create
        [Auth]
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Video video)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Category.ModifiedUser");

            video.Category = categorymanager.Find(x => x.id == video.Category.id);
            video.Channel = channelmanager.Find(x => x.id == video.Channel.id);

            if (video.Category==null || video.Channel==null)
            {
                BusinessLayerResult<Video> layerResult = new BusinessLayerResult<Video>();
                layerResult.AddError(ErrorMessageCode.ChannelorCategoryNotExist, "Kanal ya da Kategori Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kategori ya da Kanal Bulunamadı.Lütfen Kontrol Ediniz.",
                    RedirectingUrl = "/Video/Create/"
                };

                return View("Error", errorNotifyObj);
            }

            else { 
            if (ModelState.IsValid)
            {

                videomanager.Insert(video);
                videomanager.Save();
                return RedirectToAction("Index");
            }

            return View(video);
            }
        }
       
        [Auth]
        [AuthAdmin]
        // GET: Video/Edit/5
        public ActionResult Edit(int? id)
        {

        

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = videomanager.Find(x => x.id == id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }
       
        // POST: Video/Edit/5
        [Auth]
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Video video)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Category.ModifiedUser");

            video.Category = categorymanager.Find(x => x.id == video.Category.id);
            video.Channel = channelmanager.Find(x => x.id == video.Channel.id);
            if (video.Channel==null|| video.Category==null)
            {
                BusinessLayerResult<Video> layerResult = new BusinessLayerResult<Video>();
                layerResult.AddError(ErrorMessageCode.ChannelorCategoryNotExist, "Kanal ya da Kategori Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kategori ya da Kanal Bulunamadı.Lütfen Kontrol Ediniz.",
                    RedirectingUrl = "/Video/Edit/" + video.id
                };

                return View("Error", errorNotifyObj);
            }
            else { 
            if (ModelState.IsValid)
            {
                Video vid = videomanager.Find(x => x.id == video.id);
                
                vid.Videoinfo = video.Videoinfo;
                vid.Category = video.Category;
                vid.Channel = video.Channel;
                vid.Youtubeurl = video.Youtubeurl;
                vid.Likenumber = video.Likenumber;
                vid.Watchnumber = video.Watchnumber;


                videomanager.Uptade(vid);
                return RedirectToAction("Index");
            }
            return View(video);
            }
        }

        [Auth]
        [AuthAdmin]
        // GET: Video/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = videomanager.Find(x => x.id == id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }
        [AuthAdmin]
        // POST: Video/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = videomanager.Find(x => x.id == id);
            videomanager.Delete(video);
            videomanager.Save();
            return RedirectToAction("Index");
        }

        public ActionResult ListAllAdvanced()
        {
            return View(videomanager.ListQueryable().Where(x => x.Levelofvideo == "İleri" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber).ToList());
        }
        public ActionResult ListAllEasy()
        {
            return View(videomanager.ListQueryable().Where(x => x.Levelofvideo == "Kolay" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber).ToList());
        }
        public ActionResult ListAllComputer()
        {
            return View(videomanager.ListQueryable().Where(x => x.Category.Coursesubcategory == "Bilgisayar" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber).ToList());
        }
        public ActionResult ListAllMobil()
        {
            return View(videomanager.ListQueryable().Where(x => x.Category.Coursesubcategory == "Mobil" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber).ToList());
        }

        public ActionResult AllList()
        {
            return View(videomanager.ListQueryable().OrderByDescending(x => x.CreatedOn).ToList());
        }

        public ActionResult PopularBroadcast()
        {
            return View(videomanager.ListQueryable().Where(x=>x.Channel.StreamStatus==true).OrderByDescending(x => x.Watchnumber).ToList());
        }
    }
}

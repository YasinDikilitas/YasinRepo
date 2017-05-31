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
using KodlaTv.WebApp.ViewModels;
using KodlaTv.Entities.Messages;
using KodlaTv.WebApp.Filters;

namespace KodlaTv.WebApp.Controllers
{
    [Exc]
    public class ChannelController : Controller
    {
        private ChannelManager channelmanager = new ChannelManager();
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private VideoManager videomanager = new VideoManager();
        private CategoryManager categorymanager = new CategoryManager();
        private StreamerInfoManager streamermanager = new StreamerInfoManager();
        private SubscribeManager subscribemanager = new SubscribeManager();
        private ChatManager chatmanager = new ChatManager();
        // GET: Channel
        
        [Auth]
        [AuthAdmin]
        public ActionResult Index()
        {
            return View(channelmanager.ListQueryable().ToList());
        }
    
        [Auth]
        [AuthAdmin]
        // GET: Channel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Channel channel = channelmanager.Find(x => x.id == id.Value);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }
       
        [Auth]
        [AuthAdmin]
        // GET: Channel/Create
        public ActionResult Create()
        {
            return View();
        }
        [Auth]
        [AuthAdmin]
        // POST: Channel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Channel channel, HttpPostedFileBase ProfileImages)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");

            channel.Owner = kodlatvusermanager.Find(x => x.id == channel.Owner.id);
            if (channel.Owner == null)
            {
                BusinessLayerResult<Channel> layerResult = new BusinessLayerResult<Channel>();
                layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/Create"
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                channel.Owner.ModifiedUser = CurrentSession.User.Username;

                if (ModelState.IsValid)
                {
                    {
                        if (ProfileImages != null &&
                    (ProfileImages.ContentType == "image/jpeg" ||
                    ProfileImages.ContentType == "image/jpg" ||
                    ProfileImages.ContentType == "image/png"))
                        {
                            string filename = $"channel_{channel.id}.{ProfileImages.ContentType.Split('/')[1]}";

                            ProfileImages.SaveAs(Server.MapPath($"~/images/{filename}"));
                            channel.Imagefile = filename;
                        }

                        channelmanager.Insert(channel);
                        channelmanager.Save();
                        return RedirectToAction("Index");
                    }
                }

                return View(channel);
            }
        }
        [Auth]
        [AuthAdmin]
        // GET: Channel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Channel channel = channelmanager.Find(x => x.id == id.Value);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        // POST: Channel/Edit/5
        [Auth]
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Channel channel, HttpPostedFileBase ProfileImages)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");

            channel.Owner = kodlatvusermanager.Find(x => x.id == channel.Owner.id);
            if (channel.Owner == null)
            {
                BusinessLayerResult<Channel> layerResult = new BusinessLayerResult<Channel>();
                layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/Edit/" + channel.id
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                channel.Owner.ModifiedUser = CurrentSession.User.Username;
                if (ModelState.IsValid)
                {
                    if (ProfileImages != null &&
                   (ProfileImages.ContentType == "image/jpeg" ||
                   ProfileImages.ContentType == "image/jpg" ||
                   ProfileImages.ContentType == "image/png"))
                    {
                        string filename = $"channel_{channel.id}.{ProfileImages.ContentType.Split('/')[1]}";

                        ProfileImages.SaveAs(Server.MapPath($"~/images/{filename}"));
                        channel.Imagefile = filename;
                    }
                    Channel chan = channelmanager.Find(x => x.id == channel.id);

                    chan.ChannelName = channel.ChannelName;
                    chan.Imagefile = channel.Imagefile;
                    chan.StreamStatus = channel.StreamStatus;
                    chan.complain = channel.complain;
                    chan.Owner = channel.Owner;


                    channelmanager.Uptade(chan);
                    return RedirectToAction("Index");
                }
                return View(channel);
            }
        }
        [Auth]
        [AuthAdmin]
        // GET: Channel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Channel channel = channelmanager.Find(x => x.id == id.Value);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }
        [Auth]
        [AuthAdmin]
        // POST: Channel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Channel channel = channelmanager.Find(x => x.id == id);
            channelmanager.Delete(channel);
            channelmanager.Save();
            return RedirectToAction("Index");
        }



        public ActionResult Userchannel(int? id)
        {
        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var streamer = streamermanager.Find(x => x.Owner.id == id);

            Session["streamerinfo"] = streamer;
            Channel channel = channelmanager.Find(x => x.Owner.id == id);
            Session["chatchannel"] = channel;
            if (channel == null)
            {
                return HttpNotFound();
                // return RedirectToAction("CreateChannel");
            }

            return View(videomanager.ListQueryable().Where(x => x.Channel.id == channel.id).OrderByDescending(x => x.ModifyDate).ToList());
        }


        [Auth]
        public ActionResult CreateChannel()
        {
            return View();
        }
        [Auth]
        [HttpPost]
        public ActionResult CreateChannel(Channel channel, HttpPostedFileBase ProfileImages)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");
            KodlatvUser user = kodlatvusermanager.Find(x => x.id == CurrentSession.User.id);
            channel.Owner = user;
            if (channel.Owner == null)
            {
                BusinessLayerResult<Channel> layerResult = new BusinessLayerResult<Channel>();
                layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/CreateChannel"
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                channel.Owner.ModifiedUser = CurrentSession.User.Username;

                if (ModelState.IsValid)
                {
                    {
                        if (ProfileImages != null &&
                    (ProfileImages.ContentType == "image/jpeg" ||
                    ProfileImages.ContentType == "image/jpg" ||
                    ProfileImages.ContentType == "image/png"))
                        {
                            string filename = $"channel_{channel.id}.{ProfileImages.ContentType.Split('/')[1]}";

                            ProfileImages.SaveAs(Server.MapPath($"~/images/{filename}"));
                            channel.Imagefile = filename;
                        }
                        channel.complain = 0;
                        channel.StreamStatus = false;
                        channelmanager.Insert(channel);
                        channelmanager.Save();
                        return RedirectToAction("/Userchannel/"+user.id);
                    }
                }

                return View(channel);

            }
        }

        [Auth]
        public ActionResult ChatMessages()
        {
            return View();
        }

        [Auth]
        public ActionResult EditChannel()
        {

          
            Channel channel = channelmanager.Find(x => x.Owner.Username == CurrentSession.User.Username);

            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditChannel(Channel channel, HttpPostedFileBase ProfileImages)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");

            KodlatvUser user = kodlatvusermanager.Find(x => x.id == CurrentSession.User.id);
            channel.Owner = user;
            if (channel.Owner == null)
            {
                BusinessLayerResult<Channel> layerResult = new BusinessLayerResult<Channel>();
                layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/EditChannel/" + channel.id
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                channel.Owner.ModifiedUser = CurrentSession.User.Username;
                if (ModelState.IsValid)
                {
                    if (ProfileImages != null &&
                   (ProfileImages.ContentType == "image/jpeg" ||
                   ProfileImages.ContentType == "image/jpg" ||
                   ProfileImages.ContentType == "image/png"))
                    {
                        string filename = $"channel_{channel.id}.{ProfileImages.ContentType.Split('/')[1]}";

                        ProfileImages.SaveAs(Server.MapPath($"~/images/{filename}"));
                        channel.Imagefile = filename;
                    }
                    Channel chan = channelmanager.Find(x => x.id == channel.id);

                    chan.ChannelName = channel.ChannelName;
                    chan.Imagefile = channel.Imagefile;
                    chan.StreamStatus = channel.StreamStatus;
                    chan.complain = channel.complain;
                    chan.Owner = channel.Owner;


                    channelmanager.Uptade(chan);
                    return RedirectToAction("Index");
                }
                return View(channel);
            }
        }
        [Auth]
        public ActionResult UserBroadcast()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Kolay", Value = "Kolay", Selected = true });

            items.Add(new SelectListItem { Text = "Orta", Value = "Orta" });

            items.Add(new SelectListItem { Text = "İleri", Value = "İleri" });

            ViewBag.Levelofvideo = items;

            ViewBag.Categoryid = new SelectList(categorymanager.List(), "id", "Coursetitle");
            return View();
        }
        [Auth]
        [HttpPost]
        public ActionResult UserBroadcast(Video video)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Category.ModifiedUser");

            video.Channel = channelmanager.Find(x => x.Owner.id == CurrentSession.User.id);

            video.Category = categorymanager.Find(x => x.id == video.Category.id);

            if (video.Youtubeurl == null)
            {
                BusinessLayerResult<Video> layerResult = new BusinessLayerResult<Video>();
                layerResult.AddError(ErrorMessageCode.ChannelorCategoryNotExist, "Youtube url adresini boş bırakılamaz...");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Youtube Url Adresiniz Bulunamadı.Lütfen Kontrol Ediniz.",
                    RedirectingUrl = "/Channel/UserBroadcast/",
                    RedirectingTimeout = 3000
                   
                };

                return View("Error", errorNotifyObj);
            }


            if (video.Category == null || video.Channel == null)
            {
                BusinessLayerResult<Video> layerResult = new BusinessLayerResult<Video>();
                layerResult.AddError(ErrorMessageCode.ChannelorCategoryNotExist, "Kanal ya da Kategori Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kategori ya da Kanal Bulunamadı.Lütfen Kontrol Ediniz.",
                    RedirectingUrl = "/Channel/UserBroadcast/"
                };

                return View("Error", errorNotifyObj);
            }

            else
            {
                if (ModelState.IsValid)
                {
                    video.Likenumber = 0;
                    video.Watchnumber = 0;
                    video.Channel.StreamStatus = true;
                    videomanager.Insert(video);
                    videomanager.Save();
                    return RedirectToAction("/Userchannel/"+CurrentSession.User.id);
                }

                return View(video);
            }

        }
    }
}
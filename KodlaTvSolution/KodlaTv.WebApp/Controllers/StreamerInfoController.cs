using KodlaTv.BusinessLayer;
using KodlaTv.Entities;
using KodlaTv.Entities.Messages;
using KodlaTv.WebApp.Filters;
using KodlaTv.WebApp.Models;
using KodlaTv.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KodlaTv.WebApp.Controllers
{
    [Auth]
    [Exc]
    public class StreamerInfoController : Controller
    {
        private StreamerInfoManager streamerinfomanager = new StreamerInfoManager();
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        // GET: StreamerInfo
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StreamerInfo streameruser)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");

            if (ModelState.IsValid)
            {

                StreamerInfo streamer = streameruser;
                streamer.Owner = CurrentSession.User;
                streamerinfomanager.Insert(streamer);
                streamerinfomanager.Save();
                return Redirect("/Userchannel/" + CurrentSession.User.id);
            }

            return View(streameruser);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StreamerInfo streamerUser = streamerinfomanager.Find(x => x.Owner.id == id.Value);

            return View(streamerUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StreamerInfo streamerUser)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");

            streamerUser.Owner = kodlatvusermanager.Find(x => x.id == CurrentSession.User.id);

            if (streamerUser.Owner == null)
            {
                BusinessLayerResult<Channel> layerResult = new BusinessLayerResult<Channel>();
                layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/UserChannel/" + streamerUser.Owner.id
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                streamerUser.Owner.ModifiedUser = CurrentSession.User.Username;
                if (ModelState.IsValid)
                {
                 
                    StreamerInfo streamer = streamerinfomanager.Find(x => x.Owner.id == CurrentSession.User.id);
                    streamer.Experince = streamerUser.Experince;
                    streamer.Hobby = streamerUser.Hobby;
                    streamer.Usingos = streamerUser.Usingos;
                    streamer.Interest = streamerUser.Interest;
                    streamer.Name = streamer.Name;
                    streamer.Surname = streamer.Surname;

                    streamerinfomanager.Uptade(streamer);
                    return Redirect("/Userchannel/" + CurrentSession.User.id);
                }
                return View(streamerUser);
            }
        }
    }
}
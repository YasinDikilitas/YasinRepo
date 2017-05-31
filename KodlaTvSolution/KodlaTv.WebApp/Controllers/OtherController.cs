using KodlaTv.BusinessLayer;
using KodlaTv.Entities;
using KodlaTv.Entities.Messages;
using KodlaTv.Entities.ValueObjects;
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
    [Exc]
    public class OtherController : Controller
    {
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        private FollowManager followmanager = new FollowManager();
        // GET: Other
        public ActionResult Index()
        {
            return View();
        }
        [SecondaryAuth]
        public ActionResult LostPassword()
        {
            return View();
        }
        [SecondaryAuth]
        [HttpPost]
        public ActionResult LostPassword(PasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                KodlaTvUserManager bum = new KodlaTvUserManager();
                BusinessLayerResult<KodlatvUser> res = bum.ChangePasswordUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Şifre Değiştirme",
                    RedirectingUrl = "/Home/Login",
                };

                notifyObj.Items.Add("E-mail adresinize gönderdiğimiz link ile şifre değiştirme işleminizi gerçekleriniz.");

                return View("Ok", notifyObj);
            }
            return View(model);
        }
        [SecondaryAuth]
        public ActionResult ChangePassword(Guid id)
        {
            PasswordChangeModel model = new PasswordChangeModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KodlatvUser user = kodlatvusermanager.Find(x => x.ActivateGuid == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                model.PasswordGuid = id;
            }
            return View(model);
        }
        [SecondaryAuth]
        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeModel model)
        {
            if (ModelState.IsValid)
            {
                KodlatvUser user = kodlatvusermanager.Find(x => x.ActivateGuid == model.PasswordGuid);

                if (user == null)
                {
                    BusinessLayerResult<KodlatvUser> layerResult = new BusinessLayerResult<KodlatvUser>();
                    layerResult.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı.");
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = layerResult.Errors,
                        Title = "Kullanıcı Bulunamadı.",
                        RedirectingUrl = "/Other/ChangePassword"+model.PasswordGuid
                    };

                    return View("Error", errorNotifyObj);
                }
                else
                {
                    user.Password = model.Password;
                    kodlatvusermanager.Uptade(user);
                    //TODO
                    return Redirect("/Home/Login");
                }
            }
            return View(model);
        }
        [Auth]
        [HttpPost]
        public ActionResult Follow(int channelid, bool followed)
        {
            int res = 0;
            Follow follow = followmanager.Find(x => x.Channel.id == channelid && x.Owner.id == CurrentSession.User.id);
            Channel channel = channelmanager.Find(x => x.id == channelid);

            if(follow!=null && followed == false)
            {
                res=followmanager.Delete(follow);
            }
            else if(follow==null && followed == true)
            {
                res = followmanager.Insert(new Follow()
                {
                    CreatedOn = DateTime.Now,
                    Channel = channel,
                    Owner = CurrentSession.User
                    
                });
            }
            if (res > 0)
            {
                return Json(new { hasError = false, errorMessage = string.Empty, result = channel.Follows.Count });
            }
            return Json(new { hasError = true, errorMessage = "Takip etme işlemi gerçekleştirilemedi.", result = channel.Follows.Count });
        }
        [Auth]
        [HttpPost]
        public ActionResult Followed(int[] ids)
        {

            List<int> likedchannelIds = followmanager.List(
           x => x.Owner.id == CurrentSession.User.id && ids.Contains(x.Channel.id)).Select(
           x => x.Channel.id).ToList();

            return Json(new { result = likedchannelIds });
        }

        [Auth]
        [HttpPost]
        public ActionResult Complained(int[] ids)
        {

            List<int> complainedchannelIds = channelmanager.List(
           x => x.Owner.id == CurrentSession.User.id && ids.Contains(x.id)).Select(
           x => x.id).ToList();

            return Json(new { result = complainedchannelIds });
        }
        [Auth]
        [HttpPost]
        public ActionResult Complain(int channelid, bool complained)
        {
            int res = 0;
            Channel channel = channelmanager.Find(x => x.id == channelid);

            if (complained == false)
            {
                channel.complain--;
                res = channelmanager.Uptade(channel);
            }
            else if (complained == true)
            {
                channel.complain++;
                res=channelmanager.Uptade(channel);
            }
            if (res > 0)
            {
                return Json(new { hasError = false, errorMessage = string.Empty, result = channel.complain });
            }
            return Json(new { hasError = true, errorMessage = "Şikayet etme işlemi gerçekleştirilemedi.", result = channel.complain });
        }
    }
}
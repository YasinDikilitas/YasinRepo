using KodlaTv.BusinessLayer;
using KodlaTv.DataAccessLayer.EntityFramework;
using KodlaTv.Entities;
using KodlaTv.Entities.Messages;
using KodlaTv.Entities.ValueObjects;
using KodlaTv.WebApp.Filters;
using KodlaTv.WebApp.Models;
using KodlaTv.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KodlaTv.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {

        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        // GET: Home
        public ActionResult Index()
        {
           
            // BusinessLayer.Test test = new BusinessLayer.Test();
            var kategoriler = categorymanager.ListQueryable();
            var iyayin= videomanager.List(x => x.Levelofvideo == "İleri" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber);
            var byayin= videomanager.List(x => x.Levelofvideo == "Kolay" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber);
            var bilgisayaryayin= videomanager.List(x => x.Category.Coursesubcategory == "Bilgisayar" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber).ToList();
            var myayin= videomanager.List(x => x.Category.Coursesubcategory == "Mobil" && x.Channel.StreamStatus == true).OrderByDescending(x => x.Watchnumber);

            Session["Kategoriler"] = kategoriler;
            Session["İleriSeviyeYayın"] = iyayin;
            Session["BaşlangıçSeviyeYayın"] = byayin;
            Session["BilgisayarBölümü"] = bilgisayaryayin;
            Session["MobilBölümü"] = myayin;

            return View(videomanager.ListQueryable().Where(x=>x.Channel.StreamStatus==true).OrderByDescending(x => x.CreatedOn).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            KodlatvUser currentuser = Session["login"] as KodlatvUser;
            KodlaTvUserManager bom = new KodlaTvUserManager();
            BusinessLayerResult<KodlatvUser> res =
               bom.GetUserById(currentuser.id);
            if (res.Errors.Count > 0)
            {

                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            KodlatvUser currentuser = Session["login"] as KodlatvUser;
            BusinessLayerResult<KodlatvUser> res = kodlatvusermanager.GetUserById(currentuser.id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(KodlatvUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUser");
            ModelState.Remove("Username");
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.Imagefile = filename;
                }
                model.Username = CurrentSession.User.Username;
                model.Password = CurrentSession.User.Password;
                BusinessLayerResult<KodlatvUser> res = kodlatvusermanager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                Session["login"] = res.Result;
                    
                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            KodlatvUser currentuser = Session["login"] as KodlatvUser;
            BusinessLayerResult<KodlatvUser> res =
                kodlatvusermanager.RemoveUserById(currentuser.id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }
        [SecondaryAuth]
        public ActionResult Login()
        {
            return View();
        }
        [SecondaryAuth]
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                BusinessLayerResult<KodlatvUser> result = kodlatvusermanager.LoginUser(model);

                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                Session["login"] = result.Result;
                return RedirectToAction("Index");

            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        [SecondaryAuth]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult HasError()
        {
            return View();
        }
        [SecondaryAuth]
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                KodlaTvUserManager bum = new KodlaTvUserManager();
                BusinessLayerResult<KodlatvUser> res = bum.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };

                notifyObj.Items.Add("Lütfen E-mail adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınız aktive ediniz. Hesabınızı aktive etmeden canlı yayına başlamayazsınız...");

                return View("Ok", notifyObj);
            }
                return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            KodlaTvUserManager bom = new KodlaTvUserManager();
            BusinessLayerResult<KodlatvUser> res = bom.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
               
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/Login"
            };

            okNotifyObj.Items.Add("  Hesap Aktifleştirildi.Artık Canlı yayın açıp deneyimlerinizi diğer kullanıcılarla paylaşabilirsiniz.");

            return View("Ok", okNotifyObj);
        }
    }
}
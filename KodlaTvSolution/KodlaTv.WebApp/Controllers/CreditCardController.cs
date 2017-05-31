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
    public class CreditCardController : Controller
    {
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        private CreditcardManager creditcardmanager = new CreditcardManager();
        private SubscribeManager subscribemanager = new SubscribeManager();

        // GET: CreditCard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Creditcard(int? id)
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
            Session["chaids"] = channel.Owner.id;

            return PartialView("_PartialCard");
        }

        public ActionResult CreditcardSubscribe(int? id)
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
            Session["chaids"] = channel.Owner.id;

            return PartialView("_PartialSubscribe");
        }
        [HttpPost]
        public ActionResult Payment(CreditCard card) 
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");
            var chaid = Session["chaids"];
            card.Owner = kodlatvusermanager.Find(x => x.id == CurrentSession.User.id);

            if (card.Owner == null)
            {
                BusinessLayerResult<CreditCard> layerResult = new BusinessLayerResult<CreditCard>();
                layerResult.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/Userchannel/"+ chaid
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                card.Owner.ModifiedUser = CurrentSession.User.Username;
                if (card.CardName==null|| card.CardNumber == null || card.Cardlastyear == 0 || card.Cardlastmonth == 0 || card.Cardcvc==0)
                {
                    BusinessLayerResult<CreditCard> layerResult = new BusinessLayerResult<CreditCard>();
                    layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kredi Kart bilgileri bölümü boş bırakılamaz.");
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = layerResult.Errors,
                        Title = "Kredi Kart Bilgi Hatası.",
                        RedirectingTimeout = 2000,
                        RedirectingUrl = "/Channel/Userchannel/" + chaid
                    };

                    return View("Error", errorNotifyObj);
                }

                if (ModelState.IsValid)
                {
                    {
                        creditcardmanager.Insert(card);
                        creditcardmanager.Save();
                        OkViewModel okNotifyObj = new OkViewModel()
                        {
                            Title = "Bağış Yapıldı",
                            RedirectingTimeout = 4000,
                            RedirectingUrl = "/Channel/Userchannel/" + chaid
                        };

                        okNotifyObj.Items.Add("  Bağışınız için teşekkürler...");

                        return View("Ok", okNotifyObj);
                    }
                }

                return View(card);
            }

        }

        [HttpPost]
        public ActionResult Subscribe(CreditCard card)
        {
            Subscribe subscribe = new Subscribe();
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");
            var chaid = Session["chaids"];
            card.Owner = kodlatvusermanager.Find(x => x.id == CurrentSession.User.id);
            Channel channel = channelmanager.Find(x => x.Owner.id == (int)chaid);
            if (card.Owner == null || channel == null)
            {
                BusinessLayerResult<CreditCard> layerResult = new BusinessLayerResult<CreditCard>();
                layerResult.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı adı Bulunamadı.");
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = layerResult.Errors,
                    Title = "Kullanıcı Bulunamadı.",
                    RedirectingUrl = "/Channel/Userchannel/" + chaid
                };

                return View("Error", errorNotifyObj);

            }
            else
            {
                card.Owner.ModifiedUser = CurrentSession.User.Username;
                
                if (card.CardName == null || card.CardNumber == null || card.Cardlastyear == 0 || card.Cardlastmonth == 0 || card.Cardcvc == 0)
                {
                    BusinessLayerResult<CreditCard> layerResult = new BusinessLayerResult<CreditCard>();
                    layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kredi Kart bilgileri bölümü boş bırakılamaz.");
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = layerResult.Errors,
                        Title = "Kredi Kart Bilgi Hatası.",
                        RedirectingTimeout = 2000,
                        RedirectingUrl = "/Channel/Userchannel/" + chaid
                    };

                    return View("Error", errorNotifyObj);
                }

                if (ModelState.IsValid)
                {
                    {
                        subscribe.Owner = CurrentSession.User;
                        subscribe.Channel = channel;
                        subscribe.CreatedOn = DateTime.Now;
                        subscribemanager.Insert(subscribe);
                        subscribemanager.Save();
                        card.Amount = 5.00;
                        creditcardmanager.Insert(card);
                        creditcardmanager.Save();
                        OkViewModel okNotifyObj = new OkViewModel()
                        {
                            Title = "Abone Olundu",
                            RedirectingTimeout = 4000,
                            RedirectingUrl = "/Channel/Userchannel/" + chaid
                        };

                        okNotifyObj.Items.Add("  Abone oldunuz...");

                        return View("Ok", okNotifyObj);
                    }
                }

                return View(card);
            }

        }
    }
}
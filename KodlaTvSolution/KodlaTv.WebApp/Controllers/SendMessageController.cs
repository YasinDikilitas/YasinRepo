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
    public class SendMessageController : Controller
    {
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        private ChannelManager channelmanager = new ChannelManager();
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        private MessageManager messagemanager = new MessageManager();
        // GET: SendMessage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserMessagelist(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(messagemanager.ListQueryable().Where(x=>x.Owner.id==id.Value).OrderByDescending(x=>x.CreatedOn));
        }

        public ActionResult UserReceiveMessagelist(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(messagemanager.ListQueryable().Where(x => x.Recievername == CurrentSession.User.Username).OrderByDescending(x => x.CreatedOn));
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendMessage sendmessage = messagemanager.Find(x => x.id == id.Value);
            if (sendmessage == null)
            {
                return HttpNotFound();
            }
            return View(sendmessage);
        }

        // POST: Channel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SendMessage sendmessage = messagemanager.Find(x => x.id == id);
            messagemanager.Delete(sendmessage);
            messagemanager.Save();
            return Redirect("/SendMessage/UserMessagelist/" + CurrentSession.User.id);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendMessage sendmessage = messagemanager.Find(x => x.id == id.Value);
            if (sendmessage == null)
            {
                return HttpNotFound();
            }
            return View(sendmessage);
        }

        public ActionResult Sendmessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KodlatvUser kodlatvUser = kodlatvusermanager.Find(x => x.id == id.Value);
          

            if (kodlatvUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.Receiver = kodlatvUser.Username;
            if (CurrentSession.User != null)
            {
                ViewBag.Sender = CurrentSession.User.Username;
            }
            else
            {
                ViewBag.Sender = "";
            }
           
            return PartialView("_PartialMessage");
        }
        [HttpPost]
        public ActionResult Sendmessage(SendMessage sendmessage)
        {


            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("Owner.ModifiedUser");
            ModelState.Remove("Owner.Username");
            ModelState.Remove("Owner.Email");
            ModelState.Remove("Owner.Password");

            sendmessage.Owner = kodlatvusermanager.Find(x => x.Username == sendmessage.Owner.Username);
            if (sendmessage.Owner == null || sendmessage.Recievername==null)
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
                if (ModelState.IsValid)
                {
                    {
                        sendmessage.CreatedOn = DateTime.Now;
                        messagemanager.Insert(sendmessage);
                        messagemanager.Save();
                        return Redirect("/SendMessage/UserMessagelist/"+CurrentSession.User.id);
                    }
                }

                return View(sendmessage);
            }
        }
    }
}
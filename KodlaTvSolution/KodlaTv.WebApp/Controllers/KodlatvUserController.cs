using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KodlaTv.Entities;
using KodlaTv.BusinessLayer;
using KodlaTv.WebApp.Filters;

namespace KodlaTv.WebApp.Controllers
{
    [Auth]
    [Exc]
    public class KodlatvUserController : Controller
    {
        private KodlaTvUserManager kodlatvusermanager = new KodlaTvUserManager();
        [AuthAdmin]
        // GET: KodlatvUser
        public ActionResult Index()
        {
            return View(kodlatvusermanager.List());
        }
        [AuthAdmin]
        // GET: KodlatvUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KodlatvUser kodlatvUser = kodlatvusermanager.Find(x=>x.id==id.Value);
            if (kodlatvUser == null)
            {
                return HttpNotFound();
            }
            return View(kodlatvUser);
        }
        [AuthAdmin]
        // GET: KodlatvUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KodlatvUser/Create
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KodlatvUser kodlatvUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<KodlatvUser> res = kodlatvusermanager.Insert(kodlatvUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(kodlatvUser);
                }
                return RedirectToAction("Index");
            }

            return View(kodlatvUser);
        }
        [AuthAdmin]
        // GET: KodlatvUser/Edit/5
        public ActionResult Edit(int? id)
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
            return View(kodlatvUser);
        }
        [AuthAdmin]
        // POST: KodlatvUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KodlatvUser kodlatvUser)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("ModifiedUser");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<KodlatvUser> res = kodlatvusermanager.Update(kodlatvUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(kodlatvUser);
                }
                return RedirectToAction("Index");
            }
            return View(kodlatvUser);
        }
        [AuthAdmin]
        // GET: KodlatvUser/Delete/5
        public ActionResult Delete(int? id)
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
            return View(kodlatvUser);
        }
        [AuthAdmin]
        // POST: KodlatvUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KodlatvUser kodlatvUser = kodlatvusermanager.Find(x => x.id == id);
            kodlatvusermanager.Delete(kodlatvUser);
            return RedirectToAction("Index");
        }

    }
}

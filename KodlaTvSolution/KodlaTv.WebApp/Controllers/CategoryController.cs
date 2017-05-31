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
    public class CategoryController : Controller
    {
        private CategoryManager categorymanager = new CategoryManager();
        private VideoManager videomanager = new VideoManager();
        // GET: Category
        [AuthAdmin]
        public ActionResult Index()
        {
            return View(categorymanager.List());
        }

        public ActionResult ListAll()
        {
            return View(categorymanager.List());
        }
        public ActionResult Listone(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cat = categorymanager.Find(x => x.id == id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View(videomanager.ListQueryable().Where(x=>x.Category.id==cat.id&&x.Channel.StreamStatus==true).OrderByDescending(x=>x.CreatedOn));
        }
        [AuthAdmin]
        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [AuthAdmin]
        // GET: Category/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Bilgisayar", Value = "Bilgisayar", Selected = true });

            items.Add(new SelectListItem { Text = "Mobil", Value = "Mobil" });

            items.Add(new SelectListItem { Text = "Elektrik-Elektronik", Value = "Elektrik-Elektronik" });

            ViewBag.CourseSubCategory = items;

            return View();
        }

        // POST: Category/Create
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category, HttpPostedFileBase ProfileImages)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("ModifiedUser");
            if (ModelState.IsValid)
            {
                if (ProfileImages != null &&
                    (ProfileImages.ContentType == "image/jpeg" ||
                    ProfileImages.ContentType == "image/jpg" ||
                    ProfileImages.ContentType == "image/png"))
                {
                    string filename = $"category_{category.id}.{ProfileImages.ContentType.Split('/')[1]}";

                    ProfileImages.SaveAs(Server.MapPath($"~/images/{filename}"));
                    category.Imagefile = filename;
                }
                KodlatvUser currentuser = Session["login"] as KodlatvUser;
                category.ModifiedUser = currentuser.Username;
                categorymanager.Insert(category);
                categorymanager.Save();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        [AuthAdmin]
        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Bilgisayar", Value = "Bilgisayar", Selected = true });

            items.Add(new SelectListItem { Text = "Mobil", Value = "Mobil" });

            items.Add(new SelectListItem { Text = "Elektrik-Elektronik", Value = "Elektrik-Elektronik" });

            ViewBag.CourseSubCategory = items;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category, HttpPostedFileBase ProfileImages)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifyDate");
            ModelState.Remove("ModifiedUser");

            if (ModelState.IsValid)
            {
                if (ProfileImages != null &&
                 (ProfileImages.ContentType == "image/jpeg" ||
                 ProfileImages.ContentType == "image/jpg" ||
                 ProfileImages.ContentType == "image/png"))
                {
                    string filename = $"category_{category.id}.{ProfileImages.ContentType.Split('/')[1]}";

                    ProfileImages.SaveAs(Server.MapPath($"~/images/{filename}"));
                    category.Imagefile = filename;
                }
                Category cat = categorymanager.Find(x => x.id == category.id);
              
                cat.Coursetitle = category.Coursetitle;
                cat.Coursesubcategory = category.Coursesubcategory;
                cat.Imagefile = category.Imagefile;
                categorymanager.Uptade(cat);
                //TODO
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [AuthAdmin]
        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [AuthAdmin]
        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categorymanager.Find(x => x.id == id);
            categorymanager.Delete(category);
            categorymanager.Save();
            return RedirectToAction("Index");
        }

    }
}

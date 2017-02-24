using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunTravel.Models;
using System.IO;
using PagedList.Mvc;
using PagedList;
using System.Data.Entity;

namespace SunTravel.Areas.Manager.Controllers
{
    public class CountryController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        // GET: Country
        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(db.Countries.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Country country, HttpPostedFileBase uploadImage1)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage1 != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage1.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage1.ContentLength);
                    }
                    // установка массива байтов
                    country.Photo1 = imageData;
                    db.Countries.Add(country);
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Country country = db.Countries.Find(id);
            if (country != null)
            {
                return View(country);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Photo1")] Country country, string action, HttpPostedFileBase uploadImage1, HttpPostedFileBase uploadImage2, HttpPostedFileBase uploadImage3)
        {
            if (action == "Upload")
            {
                var editCountry = db.Countries
                    .Where(i => i.Id == country.Id)
                    .FirstOrDefault();

                editCountry.Id = country.Id;
                editCountry.Name = country.Name;
                editCountry.Description = country.Description;

                if (uploadImage1 != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage1.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage1.ContentLength);
                    }
                    // установка массива байтов
                    editCountry.Photo1 = imageData;
                }
             
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Country b = db.Countries.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Country b = db.Countries.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Country b = db.Countries.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Countries.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
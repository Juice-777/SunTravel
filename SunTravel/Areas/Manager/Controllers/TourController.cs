using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SunTravel.Models;
using PagedList;

namespace SunTravel.Areas.Manager.Controllers
{
    public class TourController : Controller
    {
        private SunTravelContext db = new SunTravelContext();

        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var tours = db.Tours
                .Include(t => t.Country)
                .Include(t => t.Hotel).OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);
            return View(tours);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        public ActionResult Create()
        {
            int selectedIndex = 1;
            SelectList countries = new SelectList(db.Countries, "Id", "Name", selectedIndex);
            ViewBag.Countries = countries;
            SelectList hotels = new SelectList(db.Hotels.Where(c => c.CountryId == selectedIndex), "Id", "Name");
            ViewBag.Hotels = hotels;
            return View();
        }
        public ActionResult GetItemsPartial(int id)
        {
            return PartialView(db.Hotels.Where(c => c.CountryId == id).ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Place,DateStart,Duration,Price,FreePlace,Photo1,Active,Insurance,Food,CountryId")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", tour.CountryId);
            return View(tour);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", tour.CountryId);
            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Place,DateStart,Duration,Price,FreePlace,Photo1,Active,Insurance,Food,CountryId")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", tour.CountryId);
            return View(tour);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
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

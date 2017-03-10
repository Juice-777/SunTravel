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
using System.IO;

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

            ViewBag.myButton = "Filter";
            ViewBag.getBy = "FindId";
            ViewBag.getValue = "";
            ViewBag.CountryId = 0;
            ViewBag.HotelId = 0;
            ViewBag.active = 10;
            ViewBag.food = 10;
            ViewBag.place = 10;

            //Filters: Country, Hotel, Actives, Food
            int selectedIndex = 1;
            SelectList countries = new SelectList(db.Countries.OrderBy(x => x.Name), "Id", "Name", selectedIndex);
            ViewBag.Countries = countries;
            SelectList hotels = new SelectList(db.Hotels.Where(c => c.CountryId == selectedIndex).OrderBy(x => x.Name), "Id", "Name");
            ViewBag.Hotels = hotels;

            return View(tours);
        }

        [HttpPost]
        //Confirm filters
        public ActionResult TourItemsPartial(int? page, string myButton, string getBy, string getValue, int CountryId, int HotelId, int active, int food, int place)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            //Values for AJAX paging
            ViewBag.myButton = myButton;
            ViewBag.getBy = getBy;
            ViewBag.getValue = getValue;
            ViewBag.CountryId = CountryId;
            ViewBag.HotelId = HotelId;
            ViewBag.active = active;
            ViewBag.food = food;
            ViewBag.place = place;

            var request = db.Tours.Include(x => x.Country)
                            .Include(x => x.Hotel);
            //Search by id/name
            if (myButton == "Find")
            {
                //for bntIdentity
                if (getBy != null && getValue != null)
                {
                    if (getBy == "FindId")
                    {
                        try
                        {
                            int getId = Convert.ToInt32(getValue);
                            request = request.Where(x => x.Id == getId).OrderBy(x => x.Id);
                        }
                        catch (Exception)
                        {
                            ViewBag.Msg = "Value '" + getValue + "' is not Id type!";
                            request = request.OrderBy(x => x.Id);
                        }
                    }
                    if (getBy == "FindName")
                    {
                        request = db.Tours
                                    .Where(x => x.Name == getValue.ToString())
                                    .OrderBy(x => x.Id);
                    }
                }
            }
            //Search by parameters
            if (myButton == "Filter")
            {
                
                if (CountryId != 0)
                {
                    request = request.Where(x => x.CountryId == CountryId);
                    if (HotelId != 0) //id = all hotels
                    {
                        request = request.Where(x => x.HotelId == HotelId);
                    }
                }

                //10 = id all items
                if (active != 10)
                {
                    request = request.Where(x => (int)x.Active == active);
                }
                if (food != 10)
                {
                    request = request.Where(x => (int)x.Food == food);
                }
                if (place != 10)
                {
                    request = request.Where(x => (int)x.Place == place);
                }
                request = request.OrderBy(x => x.Id);
            }
            return PartialView(request.ToPagedList(pageNumber, pageSize));
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
            SelectList countries = new SelectList(db.Countries.OrderBy(x => x.Name), "Id", "Name", selectedIndex);
            ViewBag.Countries = countries;

            SelectList hotels = new SelectList(db.Hotels.Where(c => c.CountryId == selectedIndex).OrderBy(x => x.Name), "Id", "Name");
            ViewBag.Hotels = hotels;

            return View();
        }

        //Get AJAX hotels in dropdownlist
        public ActionResult GetItemsPartial(int id)
        {
            return PartialView(db.Hotels.Where(c => c.CountryId == id).ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tour tour, HttpPostedFileBase uploadImage1)
        {
            int selectedIndex = 1;
            SelectList countries = new SelectList(db.Countries.OrderBy(x => x.Id), "Id", "Name", selectedIndex);
            ViewBag.Countries = countries;
            SelectList hotels = new SelectList(db.Hotels.Where(c => c.CountryId == selectedIndex).OrderBy(x => x.Id), "Id", "Name");
            ViewBag.Hotels = hotels;

            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);

                if (uploadImage1 != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage1.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage1.ContentLength);
                    }
                    // установка массива байтов
                    tour.Photo1 = imageData;

                    db.Tours.Add(tour);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Dropdown
            int selectedIndex = 1;
            SelectList countries = new SelectList(db.Countries.OrderBy(x => x.Id), "Id", "Name", selectedIndex);
            ViewBag.Countries = countries;

            SelectList hotels = new SelectList(db.Hotels.Where(c => c.CountryId == selectedIndex).OrderBy(x => x.Id), "Id", "Name");
            ViewBag.Hotels = hotels;

            //Initial values hotel & country of individual obj
            Tour tour = db.Tours.Find(id);
            ViewBag.CountryIdTour = tour.CountryId.ToString();
            ViewBag.HotelId = tour.HotelId.ToString();

            if (tour == null)
            {
                return HttpNotFound();
            }
            //Create list all countries
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", tour.CountryId);
            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Place,DateStart,Duration,Price,FreePlace,Photo1,Active,Insurance,Food,CountryId, HotelId")] Tour tour, HttpPostedFileBase uploadImage1)
        {
            var editTour = db.Tours.Include(x => x.Country)
                            .Include(x => x.Hotel)
                            .Where(i => i.Id == tour.Id)
                            .FirstOrDefault();

            editTour.Active = tour.Active;
            editTour.CountryId = tour.CountryId;
            editTour.DateStart = tour.DateStart;
            editTour.Duration = tour.Duration;
            editTour.Food = tour.Food;
            editTour.FreePlace = tour.FreePlace;
            editTour.HotelId = tour.HotelId;
            editTour.Insurance = editTour.Insurance;
            editTour.Name = tour.Name;
            editTour.Place = tour.Place;
            editTour.Price = tour.Price;

            if (uploadImage1 != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage1.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage1.ContentLength);
                }
                editTour.Photo1 = imageData;
            }

            db.SaveChanges();

            return RedirectToAction("Index");
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

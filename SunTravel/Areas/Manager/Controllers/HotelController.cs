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
    public class HotelController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        // GET: Hotel
        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            SelectList hotels = new SelectList(db.Hotels, "Id", "Name");
            return View(db.Hotels.Include(x => x.Country).OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult HotelItemsPartial(int? page, string getBy, string getValue)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var request = db.Hotels.Include(x => x.Country);
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
                    request = request.Where(x => x.Name == getValue).OrderBy(x => x.Name);
                }
            }

            return PartialView(request.OrderBy(x =>x.Name).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            SelectList countries = new SelectList(db.Countries, "Id", "Name");
            ViewBag.Countries = countries;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Hotel hotel, HttpPostedFileBase uploadImage1, HttpPostedFileBase uploadImage2, HttpPostedFileBase uploadImage3)
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
                    hotel.Photo1 = imageData;

                    db.Hotels.Add(hotel);
                }
                if (uploadImage2 != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage2.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage2.ContentLength);
                    }
                    hotel.Photo2 = imageData;

                    db.Hotels.Add(hotel);
                }
                if (uploadImage3 != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage3.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage3.ContentLength);
                    }
                    hotel.Photo3 = imageData;
                    db.Hotels.Add(hotel);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Hotel hotel = db.Hotels.Include(x => x.Country).Where(x => x.Id == id).FirstOrDefault();
            if (hotel != null)
            {
                SelectList hotels = new SelectList(db.Countries, "Id", "Name");
                ViewBag.Hotels = hotels;
                return View(hotel);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Photo1,Photo2,Photo3")] Hotel hotel, string action, HttpPostedFileBase uploadImage1, HttpPostedFileBase uploadImage2, HttpPostedFileBase uploadImage3)
        {
            if (action == "Upload")
            {
                var editHotel = db.Hotels.Include(x => x.Country)
                                .Where(i => i.Id == hotel.Id)
                                .FirstOrDefault();

                editHotel.Id = hotel.Id;
                editHotel.Name = hotel.Name;
                editHotel.Stars = hotel.Stars;
                editHotel.City = hotel.City;
                editHotel.CountryId = hotel.Country.Id;
                editHotel.Description = hotel.Description;

                if (uploadImage1 != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadImage1.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage1.ContentLength);
                    }
                    // установка массива байтов
                    editHotel.Photo1 = imageData;
                }

                if (uploadImage2 != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage2.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage2.ContentLength);
                    }
                    editHotel.Photo2 = imageData;

                }
                if (uploadImage3 != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage3.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage3.ContentLength);
                    }
                    editHotel.Photo3 = imageData;

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
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return Redirect("/Hotel/Details/" + id);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            db.Hotels.Remove(hotel);
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
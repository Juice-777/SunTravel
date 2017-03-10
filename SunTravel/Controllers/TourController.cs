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

namespace SunTravel.Controllers
{
    public class TourController : Controller
    {
        private SunTravelContext db = new SunTravelContext();

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var tours = db.Tours
                        .Include(t => t.Country)
                        .Include(t => t.Hotel).OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);

            ViewBag.MyButton = "Filter";
            ViewBag.CountryId = 0;
            ViewBag.getValue = "";
            ViewBag.HotelId = 0;
            ViewBag.Star = 10;
            ViewBag.Place = 10;
            ViewBag.Food = 10;

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
        public ActionResult TourItemsPartial(int? page, string myButton, int CountryId, int HotelId, int star, int place, int food)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            //Values for AJAX paging
            ViewBag.MyButton = myButton;
            ViewBag.CountryId = CountryId;
            ViewBag.HotelId = HotelId;
            ViewBag.Star = star;
            ViewBag.Place = place;
            ViewBag.Food = food;

            var request = db.Tours.Include(x => x.Country)
                            .Include(x => x.Hotel);

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
                if (star != 10)
                {
                    request = request.Where(x => x.Hotel.Stars == star);
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

                request = request.Where(x => x.Active == 0).OrderBy(x => x.Active);
            }
            return PartialView(request.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tour tour = db.Tours.Find(id);
            var tour = db.Tours.Include(x => x.Country)
                        .Include(x => x.Hotel)
                        .FirstOrDefault(x => x.Id == id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        //Get AJAX hotels in dropdownlist
        public ActionResult GetItemsPartial(int id)
        {
            return PartialView(db.Hotels.Where(c => c.CountryId == id).ToList());
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

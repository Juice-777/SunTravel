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

namespace SunTravel.Controllers
{
    [AllowAnonymous]
    public class HotelController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        // GET: Hotel
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            // Values for Filters in View
            IQueryable<Country> country = from c in db.Countries
                                           group c by c.Name into uniqueIds
                                           select uniqueIds.FirstOrDefault();
            ViewBag.Country = country.ToList();

            IQueryable<Hotel> star = from s in db.Hotels
                                        group s by s.Stars into uniqueIds
                                        select uniqueIds.FirstOrDefault();
            ViewBag.Stars = star.ToList();

            //Initial fields in view:
            ViewBag.SelectCountry = "All";
            ViewBag.SelectStars = 0;
            ViewBag.SelectSort = 0;

            return View(db.Hotels.Include(x => x.Country).OrderBy(x => x.Name).Where(x => x.Stars >2).ToPagedList(pageNumber, pageSize));
        }

        //Get filtring items
        [HttpPost] 
        public ActionResult HotelItemsPartial(int? page, string SelectCountry, string SelectStars, int SelectSort)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.SelectCountry = SelectCountry;
            ViewBag.SelectStars = Convert.ToInt32(SelectStars, 16);
            ViewBag.SelectSort = SelectSort;

            string GetCountry = SelectCountry;
            int GetStars = Convert.ToInt32(SelectStars, 16);
            int GetSort = SelectSort;

            ///Values for Filters in View
            IQueryable<Hotel> country = from c in db.Hotels
                                        group c by c.Country into uniqueIds
                                        select uniqueIds.FirstOrDefault();
            ViewBag.Country = country.ToList();

            IQueryable<Hotel> star = from s in db.Hotels
                                     group s by s.Stars into uniqueIds
                                     select uniqueIds.FirstOrDefault();
            ViewBag.Stars = star.ToList();

            /// Filtring
            var request = db.Hotels.Include(x => x.Country);

            //Sort by
            if (GetSort == 0) //All
            {
                request = request.Include(x => x.Country).OrderBy(x => x.Name);
            }
            else if (GetSort == 1) //By country
            {
                request = request.Include(x => x.Country).OrderBy(x => x.Country.Name);
            }
            else //By stars
            {
                request = request.Include(x => x.Country).OrderBy(x => x.Stars);
            }

            //Select country
            if (GetCountry == "All")
            {
                request = request.Where(x => x.Country.Name != null);
            }
            else
            {
                request = request.Where(x => x.Country.Name == GetCountry);
            }

            //Select stars
            if (GetStars != 0)
            {
                request = request.Where(x => x.Stars == GetStars);
            }
            else
            {
                request = request.Where(x => x.Stars > 0);
            }

            return PartialView(request.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Hotel b = db.Hotels
                    .Include(x => x.Country)
                    .FirstOrDefault(t => t.Id == id);

            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
    }
}
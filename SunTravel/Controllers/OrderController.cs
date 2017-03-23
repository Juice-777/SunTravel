using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunTravel.Models;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace SunTravel.Controllers
{
    public class OrderController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        // GET: Order
        [Authorize]
        public ActionResult Index (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tour tour = db.Tours.Find(id);
            var tour = db.Tours.Include(z => z.Country)
                        .Include(x => x.Hotel)
                        .FirstOrDefault(x => x.Id == id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }
        [HttpPost]
        public ActionResult Create(Tour tour, string DescriptionUser)
        {
            Order order = new Order();
            order.Status = 0;
            order.TourId = tour.Id;
            order.User = User.Identity.GetUserName();
            order.DateCreate = DateTime.Now;
            order.DescriptionUser = DescriptionUser;
            db.Orders.Add(order);

            db.SaveChanges();
            return View();
        }
    }
}
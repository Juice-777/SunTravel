using SunTravel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunTravel.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class BookingController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        // GET: Manager/Booking
        public ActionResult Index()
        {
            var booking = db.BookingForms.ToList();
            return View(booking);
        }
    }
}
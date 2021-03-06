﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunTravel.Models;
using System.IO;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace SunTravel.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Main
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "Name,Email,Country,Hotel,CheckIn,Comfort,Adults,Children,Rooms,Message")] BookingForm bookingForm)
        {
            if (ModelState.IsValid)
            {
                db.BookingForms.Add(bookingForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookingForm);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Contact()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
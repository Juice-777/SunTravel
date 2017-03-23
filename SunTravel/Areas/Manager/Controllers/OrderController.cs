using Microsoft.AspNet.Identity;
using SunTravel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SunTravel.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class OrderController : Controller
    {
        SunTravelContext db = new SunTravelContext();
        // GET: Manager/Order
        public ActionResult Index()
        {
            var order = db.Orders.ToList();
            return View(order);
        }

        //Edite order's status
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                return View(order);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            var editeOrder = db.Orders
                            .Where(i => i.Id == order.Id)
                            .FirstOrDefault();

            editeOrder.DescriptionManager = order.DescriptionManager;
            editeOrder.Manager = User.Identity.GetUserName();
            editeOrder.Status = order.Status;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                return View(order);
            }
            return HttpNotFound();
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
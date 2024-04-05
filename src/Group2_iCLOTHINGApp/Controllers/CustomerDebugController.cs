using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group2_iCLOTHINGApp.Models;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class CustomerDebugController : Controller
    {
        private Group2_iCLOTHINGDBEntities5 db = new Group2_iCLOTHINGDBEntities5();

        // GET: CustomerDebug
        public ActionResult Index()
        {
            var customer = db.Customer.Include(c => c.UserAccessLevel);
            return View(customer.ToList());
        }

        // GET: CustomerDebug/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: CustomerDebug/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.UserAccessLevel, "userID", "userAccess");
            return View();
        }

        // POST: CustomerDebug/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,customerName,customerShippingAddress,customerBillingAddress,customerDOB,customerGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(db.UserAccessLevel, "userID", "userAccess", customer.userID);
            return View(customer);
        }

        // GET: CustomerDebug/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.UserAccessLevel, "userID", "userAccess", customer.userID);
            return View(customer);
        }

        // POST: CustomerDebug/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,customerName,customerShippingAddress,customerBillingAddress,customerDOB,customerGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.UserAccessLevel, "userID", "userAccess", customer.userID);
            return View(customer);
        }

        // GET: CustomerDebug/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: CustomerDebug/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
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

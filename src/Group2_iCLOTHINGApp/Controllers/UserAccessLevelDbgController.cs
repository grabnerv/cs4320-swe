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
    public class UserAccessLevelDbgController : Controller
    {
        private Group2_iCLOTHINGDBEntities5 db = new Group2_iCLOTHINGDBEntities5();

        // GET: UserAccessLevelDbg
        public ActionResult Index()
        {
            var userAccessLevel = db.UserAccessLevel.Include(u => u.Administrator).Include(u => u.Customer).Include(u => u.UserPassword);
            return View(userAccessLevel.ToList());
        }

        // GET: UserAccessLevelDbg/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccessLevel userAccessLevel = db.UserAccessLevel.Find(id);
            if (userAccessLevel == null)
            {
                return HttpNotFound();
            }
            return View(userAccessLevel);
        }

        // GET: UserAccessLevelDbg/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.Administrator, "userId", "adminName");
            ViewBag.userID = new SelectList(db.Customer, "userID", "customerName");
            ViewBag.userID = new SelectList(db.UserPassword, "userID", "userEncryptedPassword");
            return View();
        }

        // POST: UserAccessLevelDbg/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,userAccess,userName")] UserAccessLevel userAccessLevel)
        {
            if (ModelState.IsValid)
            {
                db.UserAccessLevel.Add(userAccessLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(db.Administrator, "userId", "adminName", userAccessLevel.userID);
            ViewBag.userID = new SelectList(db.Customer, "userID", "customerName", userAccessLevel.userID);
            ViewBag.userID = new SelectList(db.UserPassword, "userID", "userEncryptedPassword", userAccessLevel.userID);
            return View(userAccessLevel);
        }

        // GET: UserAccessLevelDbg/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccessLevel userAccessLevel = db.UserAccessLevel.Find(id);
            if (userAccessLevel == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.Administrator, "userId", "adminName", userAccessLevel.userID);
            ViewBag.userID = new SelectList(db.Customer, "userID", "customerName", userAccessLevel.userID);
            ViewBag.userID = new SelectList(db.UserPassword, "userID", "userEncryptedPassword", userAccessLevel.userID);
            return View(userAccessLevel);
        }

        // POST: UserAccessLevelDbg/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,userAccess,userName")] UserAccessLevel userAccessLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userAccessLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.Administrator, "userId", "adminName", userAccessLevel.userID);
            ViewBag.userID = new SelectList(db.Customer, "userID", "customerName", userAccessLevel.userID);
            ViewBag.userID = new SelectList(db.UserPassword, "userID", "userEncryptedPassword", userAccessLevel.userID);
            return View(userAccessLevel);
        }

        // GET: UserAccessLevelDbg/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccessLevel userAccessLevel = db.UserAccessLevel.Find(id);
            if (userAccessLevel == null)
            {
                return HttpNotFound();
            }
            return View(userAccessLevel);
        }

        // POST: UserAccessLevelDbg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccessLevel userAccessLevel = db.UserAccessLevel.Find(id);
            db.UserAccessLevel.Remove(userAccessLevel);
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

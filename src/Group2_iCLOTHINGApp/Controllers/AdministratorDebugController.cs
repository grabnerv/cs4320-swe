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
    public class AdministratorDebugController : Controller
    {
        private Entities db = new Entities();

        // GET: AdministratorDebug
        public ActionResult Index()
        {
            var administrator = db.Administrator.Include(a => a.UserAccessLevel);
            return View(administrator.ToList());
        }

        // GET: AdministratorDebug/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrator.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // GET: AdministratorDebug/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.UserAccessLevel, "userID", "userAccess");
            return View();
        }

        // POST: AdministratorDebug/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userId,adminName,adminEmail,dateHired")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                db.Administrator.Add(administrator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.UserAccessLevel, "userID", "userAccess", administrator.userId);
            return View(administrator);
        }

        // GET: AdministratorDebug/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrator.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.UserAccessLevel, "userID", "userAccess", administrator.userId);
            return View(administrator);
        }

        // POST: AdministratorDebug/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,adminName,adminEmail,dateHired")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.UserAccessLevel, "userID", "userAccess", administrator.userId);
            return View(administrator);
        }

        // GET: AdministratorDebug/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrator.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: AdministratorDebug/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administrator administrator = db.Administrator.Find(id);
            db.Administrator.Remove(administrator);
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

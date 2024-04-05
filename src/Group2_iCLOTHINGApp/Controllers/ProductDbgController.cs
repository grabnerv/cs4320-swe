﻿using System;
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
    public class ProductDbgController : Controller
    {
        private Entities db = new Entities();

        // GET: ProductDbg
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Brand).Include(p => p.Category);
            return View(product.ToList());
        }

        // GET: ProductDbg/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductDbg/Create
        public ActionResult Create()
        {
            ViewBag.brandID = new SelectList(db.Brand, "brandId", "brandName");
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName");
            return View();
        }

        // POST: ProductDbg/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productID,productName,productDescription,productPrice,productQuantity,brandID,categoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brandID = new SelectList(db.Brand, "brandId", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName", product.categoryID);
            return View(product);
        }

        // GET: ProductDbg/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brandID = new SelectList(db.Brand, "brandId", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName", product.categoryID);
            return View(product);
        }

        // POST: ProductDbg/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productID,productName,productDescription,productPrice,productQuantity,brandID,categoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brandID = new SelectList(db.Brand, "brandId", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName", product.categoryID);
            return View(product);
        }

        // GET: ProductDbg/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductDbg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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

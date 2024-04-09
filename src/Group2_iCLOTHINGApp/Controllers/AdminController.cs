using Group2_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class AdminController : Controller
    {
        private Entities db = new Entities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageBillingDashboard()
        {
            // ensure user is logged in and admin
            if (Session["userID"] == null) { return RedirectToAction("Index", "User"); }
            var userID = (int)Session["userID"];
            if (db.UserAccessLevel.Find(userID).userAccess != "ADMIN") { return RedirectToAction("Index", "User"); }

            return View();
        }

        public ActionResult ProductCatalogWindow()
        {
            // ensure user is logged in and admin
            if (Session["userID"] == null) { return RedirectToAction("Index", "User"); }
            var userID = (int)Session["userID"];
            if (db.UserAccessLevel.Find(userID).userAccess != "ADMIN") { return RedirectToAction("Index", "User"); }

            if (Request.HttpMethod == "POST")
            {
                string sql = Request.Form["sql"];
                // TODO: remove parameters
                db.Database.ExecuteSqlCommand(sql);
            }
            return View();
        }
    }
}
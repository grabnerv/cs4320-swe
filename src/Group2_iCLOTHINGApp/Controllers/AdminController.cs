using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class ManageBillingController : Controller
    {

    }

    public class ProductCatalogWindowController : Controller
    {

    }

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageBillingDashboard()
        {
            return View();
        }

        public ActionResult ProductCatalogWindow()
        {
            return View();
        }
    }
}
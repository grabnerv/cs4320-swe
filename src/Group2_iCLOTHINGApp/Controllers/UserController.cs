using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group2_iCLOTHINGApp.Models;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CustomerRegistrationForm()
        {
            ViewBag.Message = "New customer registration page.";
            return View();
        }
        public ActionResult UserQueryForm()
        {
            ViewBag.Message = "User form submission page.";
            return View();
        }
        public ActionResult LandingPage()
        {
            ViewBag.Message = "User landing page.";
            return View();
        }
        public ActionResult AboutUsInformationWindow()
        {
            ViewBag.Message = "About Us page.";
            AboutUs aboutUs = GetAboutUsInfo();
            return View(aboutUs);
        }
        private AboutUs GetAboutUsInfo()
        {
            AboutUs aboutUsInfo = new AboutUs
            {
                companyAddress = "1652 Ouda Drivee",
                companyShippingPolicy = "Our shipping policy",
                companyReturnPolicy = "Our return policy",
                companyContactInfo = "mailto:Support@example.com",
                companyBusinessDescription = "Description of our business"
            };
            return aboutUsInfo;
        }
    }
}
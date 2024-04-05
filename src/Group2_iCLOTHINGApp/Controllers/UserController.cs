using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Group2_iCLOTHINGApp.Models;
using Microsoft.Ajax.Utilities;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class UserController : Controller
    {
        private Group2_iCLOTHINGDBEntities5 db = new Group2_iCLOTHINGDBEntities5();

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

        // GET: User/CustomerRegistrationForm
        public ActionResult CustomerRegistrationForm()
        {
            ViewBag.Message = "New customer registration page.";
            return View();
        }

        // POST: User/CustomerRegistrationForm
        [HttpPost]
        public ActionResult CustomerRegistrationForm([Bind(Include = "customerName,customerShippingAddress,customerBillingAddress,customerDOB,customerGender")] Customer customer)
        {
            // log sql queries
            db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            Console.WriteLine("hi from UserController");

            var password = Request.Form.Get("password");

            // we use incremental ids

            // this will be a 1-item list where the item is either null, or an int
            var maxUserIDOrNull = db.Database.SqlQuery<int?>("SELECT MAX(userID) FROM UserAccessLevel;").ToList();
            int userID;
            
            if (maxUserIDOrNull[0] == null)
            {
                userID = 1;
            }
            else
            {
                userID = maxUserIDOrNull[0].Value + 1;
            }

            customer.userID = userID;
            var ual = new UserAccessLevel
            {
                userID = customer.userID,
                userAccess = "CUSTOMER",
                userName = customer.customerName
            };

            // hash the password (ideally it would be salted + hashed)
            var hashSB = new StringBuilder();
            using (var sha = SHA256.Create())
            {
                byte[] hashB = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                foreach (byte b in hashB) { hashSB.Append(b.ToString("x2")); }
            }
            var hashedPassword = hashSB.ToString();
            
            var up = new UserPassword {
                userID = customer.userID,
                userEncryptedPassword = hashedPassword,
                passwordExpiryDate = null,
                userAccountExpiryDate = null
            };
            
            if (!ModelState.IsValid)
            {
                db.UserAccessLevel.Add(ual);
                db.SaveChanges();

                db.Customer.Add(customer);
                db.UserPassword.Add(up);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // if failed to create user, just display the form again (with error messages)

            //ViewBag.userID = new SelectList(db.UserAccessLevel, "userID", "userAccess", customer.userID);
            return View(customer);
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
        public ActionResult CustomerLoginForm()
        {
            ViewBag.Message = "Existing user login.";
            return View();
            //add authentication
        }
        public ActionResult UserCommentForm()
        {
            ViewBag.Message = "User comment form.";
            return View();
        }
    }
}
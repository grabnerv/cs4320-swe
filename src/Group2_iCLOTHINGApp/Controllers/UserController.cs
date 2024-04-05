using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Group2_iCLOTHINGApp.Models;
using Microsoft.Ajax.Utilities;

namespace Group2_iCLOTHINGApp
{
    static public class Util
    {
        public static string Sha256(string s)
        {
            var hashSB = new StringBuilder();
            using (var sha = SHA256.Create())
            {
                byte[] hashB = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
                foreach (byte b in hashB) { hashSB.Append(b.ToString("x2")); }
            }
            return hashSB.ToString();
        }
    }
}

namespace Group2_iCLOTHINGApp.Controllers
{
    public class UserController : Controller
    {
        private Group2_iCLOTHINGDBEntities5 db = new Group2_iCLOTHINGDBEntities5();

        public ActionResult Index()
        {
            return RedirectToAction("LandingPage");
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
            var maybeUserID = db.Database.SqlQuery<int?>("SELECT MAX(userID) FROM UserAccessLevel").ToList();
            int userID = 1;
            if (maybeUserID[0] != null) { userID = maybeUserID[0].Value + 1; }

            customer.userID = userID;
            var ual = new UserAccessLevel
            {
                userID = customer.userID,
                userAccess = "CUSTOMER",
                userName = customer.customerName
            };


            // hash the password (ideally it would be salted + hashed)
            var hashedPassword = Util.Sha256(password);
            var up = new UserPassword {
                userID = customer.userID,
                userEncryptedPassword = hashedPassword,
                passwordExpiryDate = null,
                userAccountExpiryDate = null
            };
            
            if (ModelState.IsValid)
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
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Message = "User form submission page.";
            return View();
        }

        [HttpPost]
        public ActionResult UserQueryForm([Bind(Include = "queryDescription")] UserQuery query)
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }

            // submit the user's query

            var maybeQueryID = db.Database.SqlQuery<int?>("SELECT MAX(queryID) FROM UserQuery").ToList();
            int queryID = 1;
            if (maybeQueryID[0] != null) { queryID = maybeQueryID[0].Value + 1; }

            query.queryID = queryID;
            query.queryDate = DateTime.Today;
            query.userID = (int)Session["userID"];

            if (ModelState.IsValid)
            {
                db.UserQuery.Add(query);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult UserCommentForm()
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Message = "User comment form.";
            return View();
        }

        [HttpPost]
        public ActionResult UserCommentForm([Bind(Include = "commentDescription")] UserComments comment)
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("Index");
            }

            // submit the user's comment

            var maybeCommentID = db.Database.SqlQuery<int?>("SELECT MAX(commentID) FROM UserComments").ToList();
            int commentID = 1;
            if (maybeCommentID[0] != null) { commentID = maybeCommentID[0].Value + 1; }

            comment.commentID = commentID;
            comment.commentDate = DateTime.Today;
            comment.userID = (int)Session["userID"];

            if (ModelState.IsValid)
            {
                db.UserComments.Add(comment);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult LandingPage()
        {
            if (Session["userID"] != null)
            {
                int userID = (int)Session["userID"];

                Console.WriteLine("grabbing customer " + userID.ToString());
                var customer = db.Customer.Find(userID);
                return View(customer);
            }
            Console.WriteLine("no login detected");
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

        // accepts GET and POST
        public ActionResult CustomerLoginForm()
        {
            if (Request.HttpMethod == "POST")
            {
                var username = Request.Form["username"];
                var password = Request.Form["password"];

                var maybeUserID = db.Database.SqlQuery<int?>("SELECT userID FROM Customer WHERE customerName = @p0", username).ToList();
                int userID;
                if (maybeUserID[0] == null)
                {
                    // error
                    return View();
                }
                else
                {
                    userID = maybeUserID[0].Value;
                }

                // validate password
                var hash = Util.Sha256(password);
                var expectedHash = db.UserPassword.Find(userID).userEncryptedPassword;
                if (hash != expectedHash)
                {
                    // error
                    return View();
                }

                // successful login
                Session["userID"] = userID;

                return RedirectToAction("Index");
            }

            ViewBag.Message = "Existing user login.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }


    }
}
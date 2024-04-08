using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group2_iCLOTHINGApp.Models;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class ShoppingCartWindowController : Controller
    {
        private Entities db = new Entities();

        // GET: ShoppingCartWindow
        public ActionResult Index()
        {
            // ensure user logged in
            if (Session["userID"] == null) { return RedirectToAction("Index", "User"); }
            int userID = (int)Session["userID"];

            List<CartItem> cartItems = ShoppingCartAPI.GetItemsInCart(db, userID);
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            // ensure user logged in
            if (Session["userID"] == null) { return RedirectToAction("Index", "User"); }
            int userID = (int)Session["userID"];

            if (db.Product.Find(productId) != null)
            {
                ShoppingCartAPI.AddToShoppingCart(db, userID, productId, 1);
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Index", "ProductList");
            }
        }
    }
}
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

        // GET and POST: ShoppingCartWindow
        public ActionResult Index()
        {
            // ensure user logged in
            if (Session["userID"] == null) { return RedirectToAction("Index", "User"); }
            int userID = (int)Session["userID"];

            if (Request.HttpMethod == "POST")
            {
                // submit an Order for each item in the cart
                List<CartItem> items = ShoppingCartAPI.GetItemsInCart(db, userID);
                int orderID = IncrementalID.getNext(db, "OrderStatus", "orderID");
                foreach (var item in items)
                {
                    var order = new OrderStatus { orderID = orderID, cartID = item.cartID, status = "NOT_SHIPPED", statusDate = DateTime.Today };
                    db.OrderStatus.Add(order);
                    orderID++;
                }
                // submitting these orders will automatically "clear" the cart from the user's end because the carts will become inactive
                db.SaveChanges();
            }
            
            List<CartItem> cartItems = ShoppingCartAPI.GetItemsInCart(db, userID);
            return View(cartItems);
        }
    }
}
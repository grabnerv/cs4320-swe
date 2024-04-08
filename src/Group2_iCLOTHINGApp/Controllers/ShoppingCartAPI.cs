using Group2_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace Group2_iCLOTHINGApp
{
    public static class IncrementalID
    {
        // Get next global incremental <cart/product/user/...> ID
        // Do not provide untrusted `table` and `idCol` parameters.
        public static int getNext(Entities db, string table, string idCol)
        {
            int id;
            // this can be 3 things: a 0-item list, a 1-item list where list[0] is null, or a 1-item list where list[0] is int
            var maybeID = db.Database.SqlQuery<int?>("SELECT MAX(" + idCol + ") FROM " + table).ToList();
            if (maybeID.Count == 0 || maybeID[0] == null)
            {
                id = 1;
            }
            else
            {
                id = maybeID[0].Value + 1;
            }
            return id;
        }
    }
}

namespace Group2_iCLOTHINGApp.Controllers
{
    // NOTE: Internally, there is one "ShoppingCart" object in the DB for each productID + user combo.
    // On the frontend, this is represented to the user as if there was one shopping cart per user.

    public static class ShoppingCartAPI
    {
        // Adds productID to cart if not already in it. Also increments quantity by the specified amount.
        // Ensure the user is logged in before calling.
        public static void AddToShoppingCart(Entities db, int userID, int productID, int quantity)
        {
            // select carts for all users by productID
            var maybeCarts = db.ShoppingCart.SqlQuery("SELECT * FROM ShoppingCart WHERE cartProductID = @p0", productID);

            // filter to the cart for the current user (if it exists)
            var maybeFilteredCart = maybeCarts.Where(sc => sc.UserAccessLevel.First().userID == userID);

            // if no such cart found
            if (maybeCarts.Count() == 0 || maybeFilteredCart.Count() == 0)
            {
                // create a cart
                int cartID = IncrementalID.getNext(db, "ShoppingCart", "cartID");

                // look up current price of item
                int productPrice = db.Product.Find(productID).productPrice.Value;

                var ual = new List<UserAccessLevel>{ db.UserAccessLevel.Find(userID) };
                ShoppingCart sc = new ShoppingCart { cartID = cartID, cartProductPrice = productPrice, cartProductQuantity = quantity, cartProductID = productID, UserAccessLevel = ual };
                db.ShoppingCart.Add(sc);
            }
            else
            {
                // update existing cart

                ShoppingCart cart = maybeFilteredCart.First(); // first and only
                cart.cartProductQuantity += quantity;
                db.Entry(cart).State = EntityState.Modified;
            }

            db.SaveChanges();

        }

        // Decrements quantity of product by specified amount. Also removes the product from cart if quantity reaches zero.
        // Ensure the user is logged in before calling.
        public static void RemoveFromShoppingCart(Entities db, int userID, int productID, int quantity)
        {
            // select carts for all users by productID
            var carts = db.ShoppingCart.SqlQuery("SELECT * FROM ShoppingCart WHERE cartProductID = @p0", productID);

            // filter to the cart for this productID for the current user (which is assumed to exist)
            var cart = carts.Where(sc => sc.UserAccessLevel.First().userID == userID).First();

            cart.cartProductQuantity -= quantity;
            if (cart.cartProductQuantity > 0)
            {
                // update the cart
                db.Entry(cart).State = EntityState.Modified;
            }
            else
            {
                // remove the cart entirely (also automatically removes the "Carts" entry)
                db.ShoppingCart.Remove(cart);
            }

            db.SaveChanges();
        }
    }
}
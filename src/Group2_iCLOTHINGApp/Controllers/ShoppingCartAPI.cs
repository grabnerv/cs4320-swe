using Group2_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Group2_iCLOTHINGApp
{
    // TODO: Move elsewhere, and refactor existing code to use this
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

    public class CartItem
    {
        public Product p;
        public int quantity;
        public int cartID;
    }
}

namespace Group2_iCLOTHINGApp
{
    // NOTE: Internally, there is one ACTIVE "ShoppingCart" object in the DB for each productID + userID combo.
    // On the frontend, this is represented to the user as if there was one active shopping cart per user.
    // There may me multiple INACTIVE ShoppingCarts for each productID + userID combo representing in-progress orders.

    // "Active" carts represent items that the user is considering ordering but have not been ordered yet.
    // "Inactive" carts represent purchased items.

    public static class ShoppingCartAPI
    {
        // Adds productID to cart if not already in it. Also increments quantity by the specified amount.
        // Ensure the user is logged in before calling.
        public static void AddToShoppingCart(Entities db, int userID, int productID, int quantity)
        {
            // select ACTIVE carts for all users by productID

            // get the carts for this user + productID combo (if they exist)
            var maybeCarts = db.ShoppingCart.SqlQuery("SELECT ShoppingCart.* FROM ShoppingCart, Carts WHERE ShoppingCart.cartID = Carts.cartID AND ShoppingCart.cartProductID = @p0 AND Carts.userID = @p1", productID, userID);

            // get the ACTIVE cart for this user + productID combo (if it exists)
            var maybeCart = maybeCarts.Where(sc => sc.OrderStatus.Count() == 0);

            //// filter to the cart for the current user (if it exists)
            //var maybeFilteredCart = maybeCarts.Where(sc => sc.UserAccessLevel.First().userID == userID);

            // if no such cart found
            if (maybeCart.Count() == 0)
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

                ShoppingCart cart = maybeCart.First(); // first and only
                cart.cartProductQuantity += quantity;
                db.Entry(cart).State = EntityState.Modified;
            }

            db.SaveChanges();

        }

        // Do not pass an inactive (ordered) cart.
        private static void ClearActiveCartById(Entities db, int cartID)
        {
            // apparently sometimes the "Carts" entry is not automatically removed first so this can error if we don't remove it
            db.Database.ExecuteSqlCommand("DELETE FROM Carts WHERE Carts.cartID = @p0", cartID);
            var cart = db.ShoppingCart.Find(cartID);
            db.ShoppingCart.Remove(cart);
            db.SaveChanges();
        }

        // Decrements quantity of product by specified amount. Also removes the product from cart if quantity reaches zero.
        // Ensure the user is logged in before calling.
        public static void RemoveFromShoppingCart(Entities db, int userID, int productID, int quantity)
        {
            // get the carts for this user + productID combo (if they exist)
            var maybeCarts = db.ShoppingCart.SqlQuery("SELECT ShoppingCart.* FROM ShoppingCart, Carts WHERE ShoppingCart.cartID = Carts.cartID AND ShoppingCart.cartProductID = @p0 AND Carts.userID = @p1", productID, userID);

            // get the ACTIVE cart for this user + productID combo (if it exists)
            var maybeCart = maybeCarts.Where(sc => sc.OrderStatus.Count() == 0);

            if (maybeCart.Count() == 0) { return; }
            var cart = maybeCart.First();

            cart.cartProductQuantity -= quantity;
            if (cart.cartProductQuantity > 0)
            {
                // update the cart
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                // remove the cart entirely
                ClearActiveCartById(db, cart.cartID);
            }
        }

        // Clears all ACTIVE carts for the user.
        public static void ClearCart(Entities db, int userID)
        {
            // select carts for user
            var maybeCarts = db.ShoppingCart.SqlQuery("SELECT ShoppingCart.* FROM ShoppingCart, Carts WHERE ShoppingCart.cartID = Carts.cartID AND Carts.userID = @p0", userID);

            // filter to ACTIVE carts
            var maybeFilteredCarts = maybeCarts.Where(sc => sc.OrderStatus.Count() == 0);

            // clear each cart
            foreach (var cart in maybeFilteredCarts)
            {
                ClearActiveCartById(db, cart.cartID);
            }
        }

        public static List<CartItem> GetItemsInCart(Entities db, int userID)
        {
            // select carts for user
            var maybeCarts = db.ShoppingCart.SqlQuery("SELECT ShoppingCart.* FROM ShoppingCart, Carts WHERE ShoppingCart.cartID = Carts.cartID AND Carts.userID = @p0", userID);

            // filter to ACTIVE carts
            var maybeFilteredCarts = maybeCarts.Where(sc => sc.OrderStatus.Count() == 0);

            // for each cart, get its associated product
            var items = new List<CartItem>();
            foreach (ShoppingCart sc in maybeFilteredCarts)
            {
                items.Add(new CartItem { p = db.Product.Find(sc.cartProductID), quantity = sc.cartProductQuantity.Value, cartID = sc.cartID });
            }
            return items;
        }
    }
}
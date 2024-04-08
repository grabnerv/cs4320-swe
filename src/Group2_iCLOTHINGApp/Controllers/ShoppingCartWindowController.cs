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
        // GET: Cart
        public ActionResult Index()
        {
            List<Product> productsInCart = GetItemsInCart();
            return View(productsInCart);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId)
        {

 
            Product product = GetProduct(productId);
            if(product != null)
            {
                AddProductToCart(product);
                return RedirectToAction("Index", "Cart");
            } else
            {
                return RedirectToAction("Index", "ProductList");
            }
            return View();
        }
        private Product GetProduct(int id)
        {
            //returning dummy product, put the database "GetProduct" function here.
            return new Product { productID = id, productName = "name", productDescription = "description" };
        }
        private ShoppingCartWindowController(Product product)
        {
            //add API logic here
        }
        private ShoppingCartWindowController()
        {
            //add Get function to see items currently in cart.
        }
    }
}
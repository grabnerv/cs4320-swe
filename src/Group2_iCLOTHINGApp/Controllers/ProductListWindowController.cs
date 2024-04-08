using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group2_iCLOTHINGApp.Models;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class ProductListWindowController : Controller
    {
        private Entities db = new Entities();

        // GET: ProductListWindow
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductListWindowView()
        {
            List<Department> departmentList = GetDepartments();
            List<Category> categoryList = GetCategories();
            List<Brand> brandList = GetBrands();
            List<Product> productList = GetProducts();
            
            ViewBag.departmentList = departmentList;
            ViewBag.categoryList = categoryList;
            ViewBag.brandList = brandList;
            ViewBag.productList = productList;
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart()
        {
            // ensure user login
            if (Session["userID"] == null) { return RedirectToAction("Index", "User"); }
            int userID = (int)Session["userID"];

            int productId = int.Parse(Request.Form["productId"]);
            if (db.Product.Find(productId) != null)
            {
                ShoppingCartAPI.AddToShoppingCart(db, userID, productId, 1);
            }

            return RedirectToAction("ProductListWindowView", "ProductListWindow");
        }

        public ActionResult DepartmentDetails(int id)
        {
            Department d = db.Department.Find(id);
            return View(d);
        }
        public ActionResult CategoryDetails(int id)
        {
            Category c = db.Category.Find(id);
            return View(c);
        }
        public ActionResult BrandDetails(int id)
        {
            Brand b = db.Brand.Find(id);
            return View(b);
        }



        private List<Department> GetDepartments()
        {
            List<Department> departments = db.Department.ToList();
            return departments;
        }
        private List<Brand> GetBrands()
        {
            List<Brand> brands = db.Brand.ToList();
            return brands;
        }
        private List<Category> GetCategories()
        {
            List<Category> categories = db.Category.ToList();
            return categories;
        }
        private List<Product> GetProducts()
        {
            List<Product> products = db.Product.ToList();
            return products;
        }
    }
}
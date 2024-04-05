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

        public ActionResult DepartmentDetails(int id)
        {
            //for returning real data from database, search the database using
            //department.id, since that is the primary key.
            //same goes for Category and Brand.
            Department d = new Department { departmentID = id, departmentDescription="Department description", departmentName="Department name" };
            return View(d);
        }
        public ActionResult CategoryDetails(int id)
        {
            Category c = new Category { categoryDescription = "Category description", categoryID = id, categoryName = "Category name" };            
            return View(c);
        }
        public ActionResult BrandDetails(int id)
        {
            Brand b = new Brand { drandDescription = "Brand description", brandId = id, brandName = "Brand name" };
            return View(b);
        }



        private List<Department> GetDepartments()
        {
            List<Department> departments = db.Department.ToList();
            //List<Department> departments = new List<Department> {
            //    new Department
            //    {
            //        departmentDescription = "Department 1 description",
            //        departmentID = 1,
            //        departmentName = "Department 1"
            //    },
            //    new Department
            //    {
            //        departmentDescription = "Department 2 description",
            //        departmentID = 2,
            //        departmentName = "Department 2"
            //    },
            //    new Department
            //    {
            //        departmentDescription = "Department 3 description",
            //        departmentID = 3,
            //        departmentName = "Department 3"
            //    }
            //};
            return departments;
        }
        private List<Brand> GetBrands()
        {
            List<Brand> brands = new List<Brand> {
                new Brand
                {
                    drandDescription = "Brand 1 description",
                    brandId = 1,
                    brandName = "Brand 1"
                },
                new Brand
                {
                    drandDescription = "Brand 2 description",
                    brandId = 2,
                    brandName = "Brand 2"
                },
                new Brand
                {
                    drandDescription = "Brand 3 description",
                    brandId = 3,
                    brandName = "Brand 3"
                }
            };
            return brands;
        }
        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category> {
                new Category
                {
                    categoryDescription = "Category 1 description",
                    categoryID = 0,
                    categoryName = "Category 1"
                },
                new Category
                {
                    categoryDescription = "Category 2 description",
                    categoryID = 1,
                    categoryName = "Category 2"
                },
                new Category
                {
                    categoryDescription = "Category 3 description",
                    categoryID = 2,
                    categoryName = "Category 3"
                }
            };
            return categories;
        }
        private List<Product> GetProducts()
        {
            List<Product> products = new List<Product> {
                new Product
                {
                    productID = 0,
                    productName = "Shoes 1",
                    productDescription = "these are some shoes",
                    productPrice = 100,
                    productQuantity = 67,
                    brandID = 1
                },
                new Product
                {
                    productID = 2,
                    productName = "Shoes 2",
                    productDescription = "these are some shoes",
                    productPrice = 100,
                    productQuantity = 87,
                    brandID = 1
                },
                new Product
                {
                    productID = 3,
                    productName = "Shoes 3",
                    productDescription = "these are some shoes",
                    productPrice = 100,
                    productQuantity = 84,
                    brandID = 1
                },
                new Product
                {
                    productID = 4,
                    productName = "Shirt 1",
                    productDescription = "these are some shirt",
                    productPrice = 100,
                    productQuantity = 13,
                    brandID = 2
                },
                new Product
                {
                    productID = 5,
                    productName = "Shirt 2",
                    productDescription = "these are some shirt",
                    productPrice = 100,
                    productQuantity = 42,
                    brandID = 0
                },
            };
            return products;
        }
    }
}
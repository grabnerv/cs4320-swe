using Group2_iCLOTHINGApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Group2_iCLOTHINGApp.Controllers
{
    public class DebugController : Controller
    {
        private Entities db = new Entities();

        public void AddProduct()
        {
            // add a product (demo)

            List<Department> departments = new List<Department>();
            Department d1 = db.Department.Find(1);
            departments.Add(d1);

            Product p = new Product
            {
                productID = 6,
                productName = "Test",
                productDescription = "Test",
                productPrice = 1000,
                productQuantity = 1,
                brandID = 1,
                categoryID = 1,
                Department = departments
            };

            db.Product.Add(p);
            db.SaveChanges();
        }

        public string GetProductDepartments(int id)
        {
            Product p = db.Product.Find(id);
            StringBuilder sb = new StringBuilder();
            foreach (Department d in p.Department)
            {
                sb.Append(d.departmentName + ", ");
            }
            string s = sb.ToString();
            return s;
        }




        // GET: Debug
        public ActionResult Index()
        {
            return View();

            //ShoppingCartAPI.AddToShoppingCart(db, 1, 1, 1);

            //ShoppingCartAPI.RemoveFromShoppingCart(db, 1, 1, 1);

            //var sql = "SELECT MAX(brandId) FROM Brand";
            //List<object> result = db.Database.SqlQuery<object>(sql).ToList();

            ////var s1 = JsonConvert.SerializeObject(result);
            //string s = "";
            //foreach (object item in result)
            //{
            //    Int32 i = item as Int32;
            //}
            //return new ContentResult { Content = s, ContentEncoding = Encoding.UTF8, ContentType = "text/plain" };
        }
    }
}

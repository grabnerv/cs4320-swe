using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Group2_iCLOTHINGApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // redirect Console writes to a file globally
            var sw = new System.IO.StreamWriter("C:\\Users\\Matt\\out.txt", true);
            sw.AutoFlush = true;
            Console.SetOut(sw);
            Console.WriteLine("hello");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

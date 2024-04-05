using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Group2_iCLOTHINGApp
{
    static public class Globals
    {
        public const bool DEBUG = true;
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            if (Globals.DEBUG)
            {
                // redirect Console writes to a file globally
                var sw = new System.IO.StreamWriter("C:\\Users\\" + Environment.UserName + "\\out.txt", true);
                sw.AutoFlush = true;
                Console.SetOut(sw);
                Console.WriteLine("starting app...");
            }

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

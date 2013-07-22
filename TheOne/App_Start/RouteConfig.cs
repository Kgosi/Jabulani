using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheOne
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Sports",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Sports", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Technology",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Technology", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Entertainment",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Entertainment", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Business",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Business", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Lifestyle",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Lifestyle", id = UrlParameter.Optional }
            );
        }
    }
}
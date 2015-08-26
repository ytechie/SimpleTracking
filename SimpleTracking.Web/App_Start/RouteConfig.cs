using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleTracking.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // /track/1z1234.rss
            routes.MapRoute(
                name: "TrackRss",
                url: "track/{trackingNumber}.rss",
                defaults: new { controller = "Track", action = "Rss" }
            );

            // /track/1z1234.json
            routes.MapRoute(
                name: "TrackJson",
                url: "track/{trackingNumber}.json",
                defaults: new { controller = "Track", action = "Json" }
            );

            // /track/1z1234.badge
            routes.MapRoute(
                name: "TrackBadge",
                url: "track/{trackingNumber}.badge",
                defaults: new { controller = "Track", action = "Badge" }
            );

            // /track/1z1234
            routes.MapRoute(
                name: "Track",
                url: "track/{id}",
                defaults: new { controller = "Track", action = "Html" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
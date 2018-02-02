using SimpleTracking.ShipperInterface.ClientServerShared;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using SimpleTracking.ShipperInterface.Tracking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SimpleTracking.Web.Controllers
{
    public class TrackController : Controller
    {
        private ITracker _tracker;

        public TrackController(ITracker tracker)
        {
            _tracker = tracker;
        }

        public ActionResult Html(string id)
        {
            var tm = new TrackModel();

            tm.TrackingNumber = id;
            tm.TrackingData = _tracker.GetTrackingData(id);

            return View(tm);
        }

        [Produces("application/json")]
        public ActionResult Rss(string trackingNumber)
        {
            var tm = new TrackModel();

            tm.TrackingNumber = trackingNumber;
            tm.TrackingData = _tracker.GetTrackingData(trackingNumber);

            var feedItems = new List<SyndicationItem>();
            
            var routeValues = new RouteValueDictionary(new { id = trackingNumber });
            var trackingLink = Url.Action("Html", "Track", routeValues, "http", Request.Url.Host);

            foreach(var activity in tm.TrackingData.Activity)
            {
                //TODO: What should these items look like?

                var itemTitle = string.Format("{0}",
                    activity.ShortDescription);

                var itemDescription = string.Format("Date/Time: {0}<br />Location: {1}<hr />"
                        + "<a href=\"http://www.SimpleTracking.com?source=feed-footer-click\"><img src=\"http://www.SimpleTracking.com/Images/Rss-Footer-Image.gif\" alt=\"Powered by SimpleTracking.com\" /></a>",
                    activity.Timestamp,
                    activity.LocationDescription,
                    activity.ShortDescription);
                
                var syndicationItem = new SyndicationItem(
                    itemTitle,
                    itemDescription,
                    new Uri(trackingLink),
                    null,
                    new DateTimeOffset(activity.Timestamp));

                syndicationItem.PublishDate = new DateTimeOffset(activity.Timestamp.ToUniversalTime());

                feedItems.Add(syndicationItem);
            }

            var feed = new SyndicationFeed(feedItems);
            feed.Title = new TextSyndicationContent(string.Format("Tracking for {0}", trackingNumber));
            feed.Description = new TextSyndicationContent(
                string.Format("Tracking details for tracking number {0}", trackingNumber));

            var syndicationLink = new SyndicationLink(new Uri(trackingLink));
            syndicationLink.RelationshipType = "alternate";
            feed.Links.Add(syndicationLink);

            return new Result(feed.GetRss20Formatter().ToString());
        }

        public JsonResult Json(string trackingNumber)
        {
            var jr = new JsonResult();
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            var tm = new TrackModel();

            tm.TrackingNumber = trackingNumber;
            tm.TrackingData = _tracker.GetTrackingData(trackingNumber);
            jr.Data = tm;

            return jr;
        }

        public string Badge(string trackingNumber)
        {
            return "<badge value=\"alert\" />";
        }
    }
}

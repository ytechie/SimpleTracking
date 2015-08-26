using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace SimpleTracking.Web.Controllers.Home
{
    public class RssActionResult : ActionResult
    {
        private readonly SyndicationFeed _feed;

        public RssActionResult(SyndicationFeed feed)
        {
            _feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var rssFormatter = new Rss20FeedFormatter(_feed, false);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output, new XmlWriterSettings { Indent = true }))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}
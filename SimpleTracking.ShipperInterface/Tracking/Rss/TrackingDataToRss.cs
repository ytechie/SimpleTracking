//using System;
//using System.Text;
//using Rss;

//namespace SimpleTracking.ShipperInterface.Tracking.Rss
//{
//    /// <summary>
//    ///		Converts <see cref="TrackingData"/> instances into RSS feeds.
//    /// </summary>
//    public class TrackingDataToRss
//    {
//        private readonly IRssBodyFormatter _bodyFormatter;
//        private readonly string _trackingPageUrlFormat;

//        /// <summary>
//        ///		Creates a new instance of the <see cref="TrackingDataToRss"/> class.
//        /// </summary>
//        /// <param name="trackingPageUrlFormat">
//        ///		The format of the link to use to track a particular tracking number. This
//        ///		link should have the "{0}" token in it, which will indicate where to
//        ///		substitute the tracking number.
//        /// </param>
//        /// <param name="bodyFormatter">
//        ///		The <see cref="IRssBodyFormatter"/> that will be used to determine the style of
//        ///		the body for each RSS item.
//        /// </param>
//        public TrackingDataToRss(string trackingPageUrlFormat, IRssBodyFormatter bodyFormatter)
//        {
//            _bodyFormatter = bodyFormatter;

//            _trackingPageUrlFormat = trackingPageUrlFormat;
//        }

//        /// <summary>
//        ///		Gets the <see cref="RssFeed"/> for the tracking data that corresponds to
//        ///		the specified tracking number.
//        /// </summary>
//        /// <param name="trackingData">
//        ///		The tracking data to display as an RSS feed.
//        /// </param>
//        /// <param name="trackingNumber">
//        ///		The tracking number that the specified <see cref="TrackingData"/> belongs to.
//        /// </param>
//        /// <returns>
//        ///		An <see cref="RssFeed"/> representing the tracking data.
//        /// </returns>
//        public RssFeed GetRssFeed(TrackingData trackingData, string trackingNumber)
//        {
//            var feed = new RssFeed {Version = RssVersion.RSS20, Encoding = Encoding.UTF8};
//            var channel = new RssChannel();
//            feed.Channels.Add(channel);
//            channel.Description = string.Format("Tracking details for tracking number {0}", trackingNumber);
//            channel.Title = string.Format("Tracking for {0}", trackingNumber);
//            channel.Link = new Uri(string.Format(_trackingPageUrlFormat, trackingNumber));
			
//            if (trackingData == null || trackingData.Activity == null || trackingData.Activity.Count == 0)
//            {
//                var currItem = new RssItem
//                {
//                    Author = "www.SimpleTracking.com",
//                    Description = "No tracking data available",
//                    Title = "No tracking data available",
//                    PubDate = DateTime.MinValue,
//                    Link = channel.Link
//                };

//                channel.Items.Add(currItem);
//            }
//            else
//            {
//                foreach (Activity currActivity in trackingData.Activity)
//                {
//                    var currItem = new RssItem
//                                    {
//                                        Author = "www.SimpleTracking.com",
//                                        Description = _bodyFormatter.GetFormattedBody(currActivity),
//                                        Title = currActivity.ShortDescription,
//                                        PubDate = currActivity.Timestamp.ToUniversalTime(),
//                                        Link = channel.Link
//                                    };

//                    channel.Items.Add(currItem);
//                }
//            }

//            return feed;
//        }
//    }
//}
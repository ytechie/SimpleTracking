using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking.Rss
{
	[TestClass]
	public class SimpleTrackingRssBodyFormatter_Tester
	{
		private SimpleTrackingRssBodyFormatter _stf;

		[TestMethod]
		public void GetFormattedBody_Verify_Html_Contents()
		{
			var activity = new Activity();
			activity.Timestamp = DateTime.Parse("2/5/06");
			activity.ShortDescription = "Picked Up";
			activity.LocationDescription = "Orlando, FL";

			_stf = new SimpleTrackingRssBodyFormatter();
			string body = _stf.GetFormattedBody(activity);
			Assert.AreEqual(
				"Date/Time: 2/5/2006 12:00:00 AM<br />Location: Orlando, FL<hr /><a href=\"http://www.SimpleTracking.com?source=feed-footer-click\"><img src=\"http://www.SimpleTracking.com/Images/Rss-Footer-Image.gif\" alt=\"Powered by SimpleTracking.com\" /></a>",
				body);
		}
	}
}
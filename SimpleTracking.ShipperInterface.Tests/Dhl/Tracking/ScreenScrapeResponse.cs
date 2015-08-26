using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	[TestClass]
	public class ScreenScrapeResponse_Tester
	{
		private string getSampleResponse(string fileName)
		{
			return
				EmbeddedFileUtilities.ReadEmbeddedTextFile(GetType().Assembly, GetType().Namespace + ".SampleScreenScrapeResponses",
				                                           fileName);
		}

		[TestMethod]
		public void Parse_Scrape_Sample1_Verify_Activities()
		{
			string xml = getSampleResponse("ScreenScrape1.txt");
			TrackingData td = ScreenScrapeResponse.GetCommonTrackingData(xml);

			Assert.AreEqual(11, td.Activity.Count);

			Assert.AreEqual(DateTime.Parse("5-8-08 12:01 am"), td.Activity[0].Timestamp);
			Assert.AreEqual("Picked up by DHL.", td.Activity[0].ShortDescription);

			Assert.AreEqual(DateTime.Parse("5-9-08 11:51 am"), td.Activity[1].Timestamp);
			Assert.AreEqual("In transit.", td.Activity[1].ShortDescription);
			Assert.AreEqual("Wilmington, OH", td.Activity[1].LocationDescription);

			Assert.AreEqual(DateTime.Parse("5-12-08 12:19 pm"), td.Activity[10].Timestamp);
			Assert.AreEqual("Bothell, WA", td.Activity[10].LocationDescription);
			Assert.AreEqual("Shipment delivered.", td.Activity[10].ShortDescription);
		}

		[TestMethod]
		public void Invalid_Tracking_Number_Response_Verify_Empty_Tracking_Data()
		{
			var scrapeData = "blah The following Tracking Number(s) are not valid blah";

			var td = ScreenScrapeResponse.GetCommonTrackingData(scrapeData);
			Assert.AreEqual(0, td.Activity.Count);
		}
	}
}
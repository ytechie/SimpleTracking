using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	[TestClass]
	public class TrackingResponse_Tester
	{
		private string getSampleResponse(string fileName)
		{
			return
				EmbeddedFileUtilities.ReadEmbeddedTextFile(GetType().Assembly, GetType().Namespace + ".SampleResponses", fileName);
		}

		[TestMethod]
		public void Parse_Sample_Response_Verify_Activities()
		{
			string xml = getSampleResponse("StandardResponse.xml");
			TrackingData td = TrackingResponse.GetCommonTrackingData(xml);

			Assert.AreEqual(5, td.Activity.Count);

			Assert.AreEqual(DateTime.Parse("4-20-2001 8:12:00 am"), td.Activity[0].Timestamp);
			Assert.AreEqual("Picked up by DHL.", td.Activity[0].ShortDescription);

			Assert.AreEqual(DateTime.Parse("2001-04-19 9:46 am"), td.Activity[1].Timestamp);
			Assert.AreEqual("NORCROSS, GA, UNITED STATES", td.Activity[1].LocationDescription);
			Assert.AreEqual("Arrived at DHL facility.", td.Activity[1].ShortDescription);

			Assert.AreEqual(DateTime.Parse("2001-04-19 6:32 pm"), td.Activity[2].Timestamp);
			Assert.AreEqual("NORCROSS, GA, UNITED STATES", td.Activity[2].LocationDescription);
			Assert.AreEqual("Departing origin.", td.Activity[2].ShortDescription);

			Assert.AreEqual(DateTime.Parse("2001-04-20 11:45 am"), td.Activity[3].Timestamp);
			Assert.AreEqual("MACON, GA, UNITED STATES", td.Activity[3].LocationDescription);
			Assert.AreEqual("Delivery Attempted.", td.Activity[3].ShortDescription);

			Assert.AreEqual(DateTime.Parse("2001-04-23 12:18 pm"), td.Activity[4].Timestamp);
			Assert.AreEqual("MACON, GA, UNITED STATES", td.Activity[4].LocationDescription);
			Assert.AreEqual("Shipment delivered.", td.Activity[4].ShortDescription);
		}

		[TestMethod]
		public void Parse_Sample_Response2_Verify_Activities()
		{
			var xml = getSampleResponse("SampleResponse2.xml");
			var td = TrackingResponse.GetCommonTrackingData(xml);

			//10 steps + 1 pickup step
			Assert.AreEqual(11, td.Activity.Count);
		}

		/// <summary>
		///		This test and sample response was added to demonstrate a bug
		///		that occurred when the time was missing from the pickup element.
		/// </summary>
		[TestMethod]
		public void Parse_Sample_Response3_Verify_Activities()
		{
			var xml = getSampleResponse("SampleResponse3.xml");
			var td = TrackingResponse.GetCommonTrackingData(xml);

			//10 steps + 1 pickup step
			//Assert.AreEqual(11, td.Activity.Count);
		}

		[TestMethod]
		public void Parse_Not_Found_Response_Verify_Null_Tracking_Data()
		{
			var xml = getSampleResponse("NotFoundResponse.xml");
			var td = TrackingResponse.GetCommonTrackingData(xml);

			Assert.IsTrue(td == null);
		}

        [TestMethod]
        public void UsageTerms()
        {
            var xml = getSampleResponse("SampleResponse3.xml");
            var td = TrackingResponse.GetCommonTrackingData(xml);
            var terms = td.UsageRequirements;

            Assert.IsTrue(terms.Length > 20);
        }
	}
}
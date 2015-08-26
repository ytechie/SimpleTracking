using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Usps.Tracking
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
		public void Parse_Multi_Step_Response_Verify_Activities()
		{
			string xmlResponse = getSampleResponse("MultipleActivities.xml");

			List<Activity> activities = TrackingResponse.GetActivities(xmlResponse);

			Assert.AreEqual(DateTime.Parse("5-9-08"), activities[0].Timestamp);
			Assert.AreEqual("Electronic Shipping Info Received", activities[0].ShortDescription);

			Assert.AreEqual(DateTime.Parse("May 09, 2008, 9:35 pm"), activities[1].Timestamp);
			Assert.AreEqual("Processed", activities[1].ShortDescription);
			Assert.AreEqual("WEST PALM BEACH, FL 33416", activities[1].LocationDescription);

			Assert.AreEqual(DateTime.Parse("May 11, 2008, 12:21 am"), activities[2].Timestamp);
			Assert.AreEqual("Processed", activities[2].ShortDescription);
			Assert.AreEqual("SAINT PAUL, MN 55101", activities[2].LocationDescription);

			Assert.AreEqual(DateTime.Parse("May 12, 2008 4:44 pm"), activities[3].Timestamp);
			Assert.AreEqual("Your item was delivered", activities[3].ShortDescription);
			Assert.AreEqual("LA FARGE, WI 54639", activities[3].LocationDescription);
		}

		[TestMethod]
		public void Parse_Multi_Step_Response_Verify_Activities_Count()
		{
			string xmlResponse = getSampleResponse("MultipleActivities.xml");

			List<Activity> activities = TrackingResponse.GetActivities(xmlResponse);

			Assert.AreEqual(4, activities.Count);
		}

		[TestMethod]
		public void Parse_Received_Detail_Verify_Activity()
		{
			Activity activity = TrackingResponse.ParseActivityString("Electronic Shipping Info Received, April 16, 2008");

			Assert.AreEqual("Electronic Shipping Info Received", activity.ShortDescription);
			Assert.AreEqual(DateTime.Parse("4-16-08"), activity.Timestamp);
		}

		[TestMethod]
		public void Parse_Step_Delivered_Verify_Activity()
		{
			Activity activity =
				TrackingResponse.ParseActivityString("Your item was delivered at 9:59 am on April 25, 2008 in RICHMOND, VA 23233.");

			Assert.AreEqual("Your item was delivered", activity.ShortDescription);
			Assert.AreEqual(DateTime.Parse("4-25-08 9:59 am"), activity.Timestamp);
			Assert.AreEqual("RICHMOND, VA 23233", activity.LocationDescription);
		}

		[TestMethod]
		public void Parse_Step_Delivered_Summary_Verify_Activity()
		{
			var activity = TrackingResponse.ParseActivityString("Your item was delivered at 5:39 pm on January 11, 2008 in KENT, WA 98031. The item was signed for by Z ALVAREZ. Additional information for this item is stored in files offline.");

			Assert.AreEqual("Your item was delivered", activity.ShortDescription);
			Assert.AreEqual(DateTime.Parse("1-11-08 5:39 pm"), activity.Timestamp);
			Assert.AreEqual("KENT, WA 98031", activity.LocationDescription);
		}

		[TestMethod]
		public void Parse_Step_Detail_Verify_Activity()
		{
			Activity activity = TrackingResponse.ParseActivityString("Processed, April 23, 2008, 10:14 pm, LAS VEGAS, NV 89119");

			Assert.AreEqual("Processed", activity.ShortDescription);
			Assert.AreEqual(DateTime.Parse("4-23-08 10:14 pm"), activity.Timestamp);
			Assert.AreEqual("LAS VEGAS, NV 89119", activity.LocationDescription);
		}

		[TestMethod]
		public void Parse_Step_Unrecognized_Verify_Activity()
		{
			Activity activity = TrackingResponse.ParseActivityString("This is an unsupported message!");
			Assert.AreEqual("This is an unsupported message!", activity.ShortDescription);
		}

		[TestMethod]
		public void Parse_Summary_Only()
		{
			var xmlResponse = getSampleResponse("SummaryOnly.xml");
			var activities = TrackingResponse.GetActivities(xmlResponse);

			Assert.AreEqual(1, activities.Count);
		}
	}
}
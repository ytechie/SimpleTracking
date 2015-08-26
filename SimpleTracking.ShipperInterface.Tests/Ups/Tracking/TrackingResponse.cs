using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Ups.Tracking
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
		public void Parse_Sample1_Verify_Activities()
		{
			var xmlResponse = getSampleResponse("trackresponse1.xml");

			var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
			var activities = trackingData.Activity;
			
			Assert.AreEqual(9, activities.Count);

			Assert.AreEqual(DateTime.Parse("2-19-01, 1:22:03 PM"), activities[0].Timestamp);
			Assert.AreEqual("US", activities[0].LocationDescription);
			Assert.AreEqual("Pickup Manifest Received", activities[0].ShortDescription);

			Assert.AreEqual(DateTime.Parse("2-21-01, 9:06 am"), activities[8].Timestamp);
			Assert.AreEqual("Roswell-Roswell, GA, US", activities[8].LocationDescription);
			Assert.AreEqual("Delivered", activities[8].ShortDescription);
		}

		[TestMethod]
		public void Parse_NoData_Verify_Null_Tracking_Data()
		{
			var xmlResponse = getSampleResponse("TrackResponse_NoData.xml");

			var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);

			Assert.IsNull(trackingData);
		}

		/// <summary>
		///		Tests an XML response that it used to choke on.
		/// </summary>
		[TestMethod]
		public void Track_Response_4_Dont_Bomb()
		{
			var xmlResponse = getSampleResponse("TrackResponse_NoEstimatedDelivery.xml");
			var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);

			Assert.AreEqual(18, trackingData.Activity.Count);
		}

		[TestMethod]
		public void Invalid_Tracking_Number_Verify_Fails_Checksum()
		{
			Assert.IsFalse(TrackingResponse.IsValidTrackingNumber("as34"));
		}

		[TestMethod]
		public void Invalid_Tracking_1Z_Number_Verify_Fails_Checksum()
		{
			Assert.IsFalse(TrackingResponse.IsValidTrackingNumber("1Z2342342356"));
		}

		[TestMethod]
		public void Valid_Tracking_1Z_Number_Verify_Passes_Checksum()
		{
			Assert.IsTrue(TrackingResponse.IsValidTrackingNumber("1Z039AF20326069009"));
		}

		[TestMethod]
		public void Valid_Tracking_1Z_Number_Verify_Passes_Checksum2()
		{
			Assert.IsTrue(TrackingResponse.IsValidTrackingNumber("1Z039AF20328374130"));
		}

		//[TestMethod]
		//public void Valid_Tracking_Number_Verify_Passes_Checksum()
		//{
		//  Assert.IsTrue(TrackingResponse.IsValidTrackingNumber("987654312345672"));
		//}

		[TestMethod]
		public void Invalid_Tracking_Number_Length()
		{
			Assert.IsFalse(TrackingResponse.IsValidTrackingNumber("987654312345672"));
		}

		[TestMethod]
		public void Cross_Reference_First_Letter_Verify_2()
		{
			Assert.AreEqual(2, TrackingResponse.GetTrackingNumberCrossReferenceValue('A'));
			Assert.AreEqual(2, TrackingResponse.GetTrackingNumberCrossReferenceValue('a'));
		}

		[TestMethod]
		public void Cross_Reference_Second_Letter_Verify_3()
		{
			Assert.AreEqual(3, TrackingResponse.GetTrackingNumberCrossReferenceValue('B'));
			Assert.AreEqual(3, TrackingResponse.GetTrackingNumberCrossReferenceValue('b'));
		}

		[TestMethod]
		public void Cross_Reference_Last_Letter_In_First_Group_Verify_9()
		{
			Assert.AreEqual(9, TrackingResponse.GetTrackingNumberCrossReferenceValue('H'));
			Assert.AreEqual(9, TrackingResponse.GetTrackingNumberCrossReferenceValue('h'));
		}

		[TestMethod]
		public void Cross_Reference_First_Letter_In_Second_Group_Verify_0()
		{
			Assert.AreEqual(0, TrackingResponse.GetTrackingNumberCrossReferenceValue('I'));
			Assert.AreEqual(0, TrackingResponse.GetTrackingNumberCrossReferenceValue('i'));
		}

		[TestMethod]
		public void Cross_Reference_Last_Letter_Verify_7()
		{
			Assert.AreEqual(7, TrackingResponse.GetTrackingNumberCrossReferenceValue('Z'));
			Assert.AreEqual(7, TrackingResponse.GetTrackingNumberCrossReferenceValue('z'));
		}

		[TestMethod]
		public void Convert_To_Numeric()
		{
			Assert.AreEqual("122334", TrackingResponse.ConvertToNumeric("1a2b3c"));
		}

        [TestMethod]
        public void Parse_RealResponse_2013_07_21_Verify_Activities()
        {
            var xmlResponse = getSampleResponse("RealResponse_2013-07-21.xml");

            var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
            var activities = trackingData.Activity;

            Assert.AreEqual(6, activities.Count);

            Assert.AreEqual(DateTime.Parse("2013-07-18, 18:57:00"), activities[5].Timestamp);
            Assert.AreEqual("Denmark, WI, US", activities[5].LocationDescription);
            Assert.AreEqual("Delivered", activities[5].ShortDescription);
        }

        [TestMethod]
        public void Parse_Sample1_ReferenceNumbers()
        {
            var xmlResponse = getSampleResponse("trackresponse1.xml");

            var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
            Assert.AreEqual(2, trackingData.ReferenceNumbers.Count);
            Assert.AreEqual("CUSTOMER SUPPLIED REFERENCE NUMBER", trackingData.ReferenceNumbers[0]);
            Assert.AreEqual("TEST", trackingData.ReferenceNumbers[1]);
        }

        [TestMethod]
        public void Parse_Sample1_ServiceType()
        {
            var xmlResponse = getSampleResponse("trackresponse1.xml");

            var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
            Assert.AreEqual("NEXT DAY AIR", trackingData.ServiceType);
        }

        [TestMethod]
        public void Parse_Sample1_Weight()
        {
            var xmlResponse = getSampleResponse("trackresponse1.xml");

            var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
            Assert.AreEqual(1.7M, trackingData.Weight);
        }

        [TestMethod]
        public void Parse_Sample1_LastUpdatedRecent()
        {
            var xmlResponse = getSampleResponse("trackresponse1.xml");

            var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
            Assert.IsTrue(trackingData.LastUpdated > DateTime.Now.AddSeconds(-5));
        }

        [TestMethod]
        public void UsageTerms()
        {
           var xmlResponse = getSampleResponse("trackresponse1.xml");
           var trackingData = TrackingResponse.GetCommonTrackingData(xmlResponse);
           var terms = trackingData.UsageRequirements;

           Assert.IsTrue(terms.Length > 20);
        }

	    [TestMethod]
	    public void EnsureStagesPopulated()
	    {
            var xmlResponse = getSampleResponse("trackresponse1.xml");

            var activity = TrackingResponse.GetCommonTrackingData(xmlResponse).Activity.OrderBy(x => x.Timestamp).ToList();

            Assert.AreEqual(ShipmentStage.Created, activity[0].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[1].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[2].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[3].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[4].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[5].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[6].Stage);
            Assert.AreEqual(ShipmentStage.Scan, activity[7].Stage);
            Assert.AreEqual(ShipmentStage.Delivered, activity[8].Stage); 
	    }
	}
}
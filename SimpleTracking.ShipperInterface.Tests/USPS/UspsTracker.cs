using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using SimpleTracking.ShipperInterface.Usps.Tracking;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Usps
{
	[TestClass]
	public class UspsTracker_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			mocks = new MockRepository();
			_postUtil = mocks.CreateMock<IWebPoster>();

			_tracker = new UspsTracker(_postUtil, "userName", "password", true);
		}

		#endregion

		private UspsTracker _tracker;

		private MockRepository mocks;
		private IWebPoster _postUtil;

		private string getSampleResponse(string fileName)
		{
			return
				EmbeddedFileUtilities.ReadEmbeddedTextFile(GetType().Assembly, GetType().Namespace + ".Tracking.SampleResponses",
				                                           fileName);
		}

		[TestMethod]
		public void Change_Digit_Verify_Invalid_Checksum()
		{
			Assert.AreEqual(false, UspsTracker.IsUspsTrackingNumber("9101805214907598388159"));
		}

		[TestMethod]
		public void Invalid_Tracking_Number_Length()
		{
			Assert.AreEqual(false, UspsTracker.IsUspsTrackingNumber("910180521490759838811"));
		}

		[TestMethod]
		public void Invalid_Tracking_Number_Verify_Null_Tracking_Data_Returned()
		{
			_tracker = new UspsTracker(_postUtil, "userName", "password", false);

			Assert.IsNull(_tracker.GetTrackingData("123"));
		}

		[TestMethod]
		public void Test_Dev_Url_Request_Response_Process()
		{
			_tracker = new UspsTracker(_postUtil, "userName", "password", false);

			Expect.Call(_postUtil.PostData(null, null)).IgnoreArguments()
				.Constraints(Text.StartsWith("http://testing.shippingapis.com/ShippingAPITest.dll?API=TrackV2&XML=<TrackRequest"),
				             Is.Null())
				.Return(getSampleResponse("MultipleActivities.xml"));

			mocks.ReplayAll();

			TrackingData td = _tracker.GetTrackingData("9101805213907598388159");

			Assert.AreEqual(4, td.Activity.Count);

			mocks.VerifyAll();
		}

		[TestMethod]
		public void Test_Overall_Request_Response_Process()
		{
			Expect.Call(_postUtil.PostData(null, null)).IgnoreArguments()
				.Constraints(Text.StartsWith("http://production.shippingapis.com/ShippingAPI.dll?API=TrackV2&XML=<TrackRequest"),
				             Is.Null())
				.Return(getSampleResponse("MultipleActivities.xml"));

			mocks.ReplayAll();

			TrackingData td = _tracker.GetTrackingData("9101805213907598388159");

			Assert.AreEqual(4, td.Activity.Count);

			mocks.VerifyAll();
		}

		[TestMethod]
		public void Valid_Tracking_Number_Verify_20_Digit_Checksum()
		{
			Assert.AreEqual(true, UspsTracker.IsUspsTrackingNumber("03071790000449119786"));
		}

		[TestMethod]
		public void Valid_Tracking_Number_Verify_Checksum()
		{
			Assert.AreEqual(true, UspsTracker.IsUspsTrackingNumber("9101123456789000000013"));
		}

		[TestMethod]
		public void Valid_Tracking_Number_Verify_Checksum2()
		{
			Assert.AreEqual(true, UspsTracker.IsUspsTrackingNumber("9101805213907598388159"));
		}

        [TestMethod]
        public void UsageTerms()
        {
            var xmlResponse = getSampleResponse("SummaryOnly.xml");
            var td = TrackingResponse.GetCommonTrackingData(xmlResponse);
            var terms = td.UsageRequirements;

            Assert.IsTrue(terms.Length > 20);
        }
	}
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	[TestClass]
	public class DhlTracker_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void Setup()
		{
			mocks = new MockRepository();
			_postUtil = mocks.CreateMock<IWebPoster>();

			_dt = new DhlTracker(_postUtil, "user", "pass");
		}

		#endregion

		private DhlTracker _dt;

		private MockRepository mocks;
		private IWebPoster _postUtil;

		private string getSampleResponse(string fileName)
		{
			return
				EmbeddedFileUtilities.ReadEmbeddedTextFile(GetType().Assembly, GetType().Namespace + ".SampleResponses", fileName);
		}

		[TestMethod]
		public void Test_Overall_Request_Response_Process()
		{
			Expect.Call(_postUtil.PostData(null, null)).IgnoreArguments()
				.Constraints(Is.Equal("HTTPS://eCommerce.Airborne.com/APILandingTest.asp"), Is.NotNull())
				.Return(getSampleResponse("StandardResponse.xml"));

			mocks.ReplayAll();

			TrackingData td = _dt.GetTrackingData("12991078030");

			Assert.AreEqual(5, td.Activity.Count);

			mocks.VerifyAll();
		}

		[TestMethod]
		public void Tracking_Number_Too_Long_Verify_Invalid()
		{
			Assert.IsFalse(DhlTracker.IsValidTrackingNumber("129910788343"));
		}

		[TestMethod]
		public void Tracking_Number_Too_Short_Verify_Invalid()
		{
			Assert.IsFalse(DhlTracker.IsValidTrackingNumber("129910788"));
		}

		[TestMethod]
		public void Valid_10_Digit_Tracking_Number()
		{
			//Not sure if this is ACTUALLY valid, but the length is right
			Assert.IsTrue(DhlTracker.IsValidTrackingNumber("1299107803"));
		}

		[TestMethod]
		public void Valid_11_Digit_Tracking_Number()
		{
			Assert.IsTrue(DhlTracker.IsValidTrackingNumber("12991078030"));
		}
	}
}
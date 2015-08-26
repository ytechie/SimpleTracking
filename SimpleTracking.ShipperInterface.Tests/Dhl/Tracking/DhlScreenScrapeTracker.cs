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
	public class DhlScreenScrapeTracker_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void Setup()
		{
			mocks = new MockRepository();
			_postUtil = mocks.CreateMock<IWebPoster>();

			_dt = new DhlScreenScrapeTracker(_postUtil);
		}

		#endregion

		private DhlScreenScrapeTracker _dt;

		private MockRepository mocks;
		private IWebPoster _postUtil;

		private string getSampleResponse(string fileName)
		{
			return
				EmbeddedFileUtilities.ReadEmbeddedTextFile(GetType().Assembly, GetType().Namespace + ".SampleScreenScrapeResponses",
				                                           fileName);
		}

		[TestMethod]
		public void Test()
		{
			var html = getSampleResponse("ScreenScrape2.txt");
			var xml = ScreenScrapeResponse.GetDhlTrackingXml(html);

			Assert.IsTrue(xml.Length > 50);
		}

		[TestMethod]
		public void Test_Overall_Request_Response_Process()
		{
			Expect.Call(_postUtil.PostData(null, null)).IgnoreArguments()
				.Constraints(Is.Equal("http://track.dhl-usa.com/TrackByNbr.asp?nav=Tracknbr"), Is.Equal("txtTrackNbrs=12991078030"))
				.Return(getSampleResponse("ScreenScrape1.txt"));

			mocks.ReplayAll();

			TrackingData td = _dt.GetTrackingData("12991078030");

			Assert.AreEqual(11, td.Activity.Count);

			mocks.VerifyAll();
		}

		[TestMethod]
		public void Track_Invalid_Number_Verify_Null_Tracking_Data()
		{
			TrackingData td = _dt.GetTrackingData("1299107");
			Assert.AreEqual(null, td);
		}
	}
}
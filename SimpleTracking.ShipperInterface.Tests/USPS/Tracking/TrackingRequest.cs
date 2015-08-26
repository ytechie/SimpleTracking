using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Usps.Tracking
{
	[TestClass]
	public class TrackingRequest_Tester
	{
		private string getSampleRequest(string fileName)
		{
			return
				EmbeddedFileUtilities.ReadEmbeddedTextFile(GetType().Assembly, GetType().Namespace + ".SampleRequests", fileName);
		}

		[TestMethod]
		public void Serialize()
		{
			string req = TrackingRequest.GetTrackingRequest("testTrackingNumber", "testUserName", "testPass");
			string expected = getSampleRequest("Request1.xml");

			Assert.AreEqual(expected, req);
		}
	}
}
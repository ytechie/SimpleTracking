using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
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
		public void Get_Tracking_Request_Verify_Substitutions()
		{
			string req = TrackingRequest.GetTrackingRequest("12345", "user", "pass");
			string expected = getSampleRequest("SampleRequest1.xml");

			Assert.AreEqual(expected, req);
		}
	}
}
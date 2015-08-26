using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class TrackingException_Tester
	{
		private TrackingException _te;

		[TestMethod]
		public void Constructor_Sets_TrackingNumber()
		{
			_te = new TrackingException("324a");

			Assert.AreEqual("324a", _te.TrackingNumber);
		}

		[TestMethod]
		public void Constructor_Sets_TrackingNumber_And_Message()
		{
			_te = new TrackingException("asd2a", "wejklasd");

			Assert.AreEqual("asd2a", _te.TrackingNumber);
			Assert.AreEqual("wejklasd", _te.Message);
		}

		[TestMethod]
		public void Get_And_Set_Tracking_Number()
		{
			_te = new TrackingException("324a");

			_te.TrackingNumber = "asdf3";
			Assert.AreEqual("asdf3", _te.TrackingNumber);
		}
	}
}
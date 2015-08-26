using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class TrackingNumberUtilities_Tester
	{
		[TestMethod]
		public void Dont_Alter_Valid_Tracking_Number()
		{
			Assert.AreEqual("abc123", TrackingNumberUtilities.CleanTrackingNumber("abc123", true));
		}

		[TestMethod]
		public void Remove_Inner_Spaces()
		{
			Assert.AreEqual("1Z2F87X06857107615", TrackingNumberUtilities.CleanTrackingNumber("1Z 2F8 7X0 68 5710 7615", true));
		}

		[TestMethod]
		public void Remove_Leading_And_Trailing_Spaces()
		{
			Assert.AreEqual("abc123", TrackingNumberUtilities.CleanTrackingNumber(" abc123 ", true));
		}

		[TestMethod]
		public void Inner_Letters_Dont_Remove()
		{
			Assert.AreEqual("123abc123", TrackingNumberUtilities.CleanTrackingNumber("123abc123", false));
		}

		[TestMethod]
		public void Remove_Plus_Characters()
		{
			Assert.AreEqual("abc123", TrackingNumberUtilities.CleanTrackingNumber("abc123+", true));
		}

		[TestMethod]
		public void Clean_Null_Tracking_Number_Dont_Bomb()
		{
			Assert.AreEqual("", TrackingNumberUtilities.CleanTrackingNumber(null, true));
		}

		[TestMethod]
		public void Clean_Empty_Tracking_Number_Dont_Bomb()
		{
			Assert.AreEqual("", TrackingNumberUtilities.CleanTrackingNumber("", true));
		}

		[TestMethod]
		public void Clean_Empty_Tracking_Number_Dont_Bomb2()
		{
			Assert.AreEqual("", TrackingNumberUtilities.CleanTrackingNumber(" ", true));
		}
	}
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface
{
	[TestClass]
	public class ShipperInterfaceException_Tester
	{
		private ShipperInterfaceException _sie;

		[TestMethod]
		public void Constructor_Sets_Message()
		{
			_sie = new ShipperInterfaceException("adfgh");
			Assert.AreEqual("adfgh", _sie.Message);
		}

		[TestMethod]
		public void Empty_Constructor()
		{
			_sie = new ShipperInterfaceException();
			Assert.AreEqual("", _sie.Message);
		}
	}
}
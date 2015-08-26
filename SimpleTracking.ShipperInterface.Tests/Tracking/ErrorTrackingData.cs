using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class ErrorTrackingData_Tester
	{
		private ErrorTrackingData _etd;

		[TestMethod]
		public void Set_Exception_Verify_Get_Exception()
		{
			var ex = new Exception();
			_etd = new ErrorTrackingData(ex);
			Assert.AreEqual(ex, _etd.Exception);
		}
	}
}
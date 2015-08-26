using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface.ClientServerShared
{
	[TestClass]
	public class Activity_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			_a = new Activity();
		}

		#endregion

		private Activity _a;

		[TestMethod]
		public void Set_And_Get_LocationDescription()
		{
			_a.LocationDescription = "Green Bay, WI";
			Assert.AreEqual("Green Bay, WI", _a.LocationDescription);
		}

		[TestMethod]
		public void Set_And_Get_ShortDescription()
		{
			_a.ShortDescription = "test short desc";
			Assert.AreEqual("test short desc", _a.ShortDescription);
		}

		[TestMethod]
		public void Set_And_Get_Timestamp()
		{
			DateTime testDate = DateTime.Parse("3-6-06 5:45 pm");

			_a.Timestamp = testDate;
			Assert.AreEqual(testDate, _a.Timestamp);
		}
	}
}
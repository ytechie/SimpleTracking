using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks.Constraints;

namespace SimpleTracking.ShipperInterface.ClientServerShared
{
	[TestClass]
	public class TrackingData_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			_td = new TrackingData();
		}

		#endregion

		private TrackingData _td;

		[TestMethod]
		public void Activity()
		{
			var activities = new List<Activity>();

			_td.Activity = activities;
			Assert.AreEqual(activities, _td.Activity);
		}

		[TestMethod]
		public void EstimatedDelivery()
		{
			DateTime now = DateTime.Now;

			_td.EstimatedDelivery = now;
			Assert.AreEqual(now, _td.EstimatedDelivery);
		}

		[TestMethod]
		public void TrackerName()
		{
			_td.TrackerName = "FedEx";
			Assert.AreEqual("FedEx", _td.TrackerName);
		}

		[TestMethod]
		public void UsageRequirements()
		{
			_td.UsageRequirements = "Don't use, ever";
			Assert.AreEqual("Don't use, ever", _td.UsageRequirements);
		}

	    [TestMethod]
	    public void NullActivity_NoLatestStage()
	    {
	        _td.Activity = null;
            Assert.AreEqual(null, _td.GetCurrentShipmentStage());
	    }

        [TestMethod]
        public void NoActivity_NoLatestStage()
        {
            _td.Activity = new List<Activity>();
            Assert.AreEqual(null, _td.GetCurrentShipmentStage());
        }

        [TestMethod]
        public void Activity_LatestStage()
        {
            _td.Activity = new List<Activity>
            {
                new Activity { Timestamp = DateTime.Parse("2013-10-10 8:00"), Stage = ShipmentStage.Created },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 9:00"), Stage = ShipmentStage.Scan },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 10:00"), Stage = ShipmentStage.Delivered }
            };
            Assert.AreEqual(ShipmentStage.Delivered, _td.GetCurrentShipmentStage());
        }

        [TestMethod]
        public void ActivityMissingLatestStage_NullStage()
        {
            _td.Activity = new List<Activity>
            {
                new Activity { Timestamp = DateTime.Parse("2013-10-10 8:00"), Stage = ShipmentStage.Created },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 9:00"), Stage = ShipmentStage.Scan },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 10:00"), Stage = null }
            };
            Assert.AreEqual(null, _td.GetCurrentShipmentStage());
        }

        [TestMethod]
        public void CurrentActivityStatus_NullActivity_NullMessage()
        {
            _td.Activity = null;
            Assert.AreEqual(null, _td.GetCurrentActivityStatus());
        }

        [TestMethod]
        public void CurrentActivityStatus_NoActivity_NullMessage()
        {
            _td.Activity = new List<Activity>();
            Assert.AreEqual(null, _td.GetCurrentActivityStatus());
        }

        [TestMethod]
        public void CurrentActivityStatus_Activity()
        {
            _td.Activity = new List<Activity>
            {
                new Activity { Timestamp = DateTime.Parse("2013-10-10 8:00"), ShortDescription = "1"},
                new Activity { Timestamp = DateTime.Parse("2013-10-10 9:00"), ShortDescription = "2" },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 10:00"), ShortDescription = "3" }
            };
            Assert.AreEqual("3", _td.GetCurrentActivityStatus());
        }

        [TestMethod]
        public void CurrentActivityStatus_ActivityMissingLatestStage_NullStage()
        {
            _td.Activity = new List<Activity>
            {
                new Activity { Timestamp = DateTime.Parse("2013-10-10 8:00"), ShortDescription = "1" },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 9:00"), ShortDescription = "2" },
                new Activity { Timestamp = DateTime.Parse("2013-10-10 10:00"), ShortDescription = null }
            };
            Assert.AreEqual(null, _td.GetCurrentActivityStatus());
        }
	}
}
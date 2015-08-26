//using System.Collections.Generic;
//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Rhino.Mocks;
//using Rss;
//using SimpleTracking.ShipperInterface.Tracking.Simulation;

//namespace SimpleTracking.ShipperInterface.Tracking.Rss
//{
//    [TestClass]
//    public class TrackingDataToRss_Tester
//    {
//        #region Setup/Teardown

//        [TestInitialize]
//        public void SetUp()
//        {
//            _mocks = new MockRepository();
//            _mockBodyFormatter = _mocks.CreateMock<IRssBodyFormatter>();
//        }

//        #endregion

//        private TrackingDataToRss _tdr;

//        private MockRepository _mocks;
//        private IRssBodyFormatter _mockBodyFormatter;

//        [TestMethod]
//        public void Get_Rss_Feed_Verify_Contents()
//        {
//            var st = new SimulationTracker();
//            TrackingData td = st.GetTrackingData("sim1");

//            Expect.Call(_mockBodyFormatter.GetFormattedBody(td.Activity[0])).Return("one");
//            Expect.Call(_mockBodyFormatter.GetFormattedBody(td.Activity[1])).Return("two");

//            _mocks.ReplayAll();

//            _tdr = new TrackingDataToRss("http://test.com/?tn={0}", _mockBodyFormatter);
//            RssFeed feed = _tdr.GetRssFeed(td, "sim1");
//            Assert.AreEqual("http://test.com/?tn=sim1", feed.Channels[0].Link.ToString());
//            Assert.AreEqual(2, feed.Channels[0].Items.Count);
//            foreach (RssItem currItem in feed.Channels[0].Items)
//            {
//                Assert.AreEqual("http://test.com/?tn=sim1", currItem.Link.ToString());
//            }

//            Assert.AreEqual("one", feed.Channels[0].Items[0].Description);
//            Assert.AreEqual("two", feed.Channels[0].Items[1].Description);

//            //Verify that it's writable
//            var ms = new MemoryStream();
//            feed.Write(ms);

//            _mocks.VerifyAll();
//        }

//        [TestMethod]
//        public void Get_Rss_From_Null_Tracking_Data_Verify_No_Exception()
//        {
//            _mocks.ReplayAll();

//            _tdr = new TrackingDataToRss("http://test.com/?tn={0}", _mockBodyFormatter);
//            RssFeed feed = _tdr.GetRssFeed(null, "asdf");
//            Assert.AreEqual(1, feed.Channels[0].Items.Count);

//            //Verify that it's writable
//            var ms = new MemoryStream();
//            feed.Write(ms);
			
//            _mocks.VerifyAll();
//        }

//        [TestMethod]
//        public void Get_Rss_From_Empty_Tracking_Data_Verify_No_Exception()
//        {
//            var td = new TrackingData();
//            td.TrackerName = "Asdfas";

//            _mocks.ReplayAll();

//            _tdr = new TrackingDataToRss("http://test.com/?tn={0}", _mockBodyFormatter);
//            RssFeed feed = _tdr.GetRssFeed(td, "asdf");
//            Assert.AreEqual(1, feed.Channels[0].Items.Count);

//            //Verify that it's writable
//            var ms = new MemoryStream();
//            feed.Write(ms);

//            _mocks.VerifyAll();
//        }

//        [TestMethod]
//        public void Get_Rss_From_No_Activity_Verify_No_Exception()
//        {
//            var td = new TrackingData();
//            td.TrackerName = "Asdfas";
//            td.Activity = new List<Activity>();

//            _mocks.ReplayAll();

//            _tdr = new TrackingDataToRss("http://test.com/?tn={0}", _mockBodyFormatter);
//            RssFeed feed = _tdr.GetRssFeed(td, "asdf");
//            Assert.AreEqual(1, feed.Channels[0].Items.Count);

//            //Verify that it's writable
//            var ms = new MemoryStream();
//            feed.Write(ms);

//            _mocks.VerifyAll();
//        }
//    }
//}
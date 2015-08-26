using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SimpleTracking.ShipperInterface.Tracking.Http;

namespace SimpleTracking.ShipperInterface.Common.Http
{
	[TestClass]
	public class PackageTrackrRetryPostUtility_Tester
	{
		private PackageTrackrRetryPostUtility _rpu;
		private IWebPoster _mockPoster;
		private IThreadSleeper _mockThreadSleeper;

		[TestInitialize]
		public void SetUp()
		{
			_mockPoster = MockRepository.GenerateStub<IWebPoster>();
			_mockThreadSleeper = MockRepository.GenerateStub<IThreadSleeper>();

			_rpu = new PackageTrackrRetryPostUtility(_mockPoster, _mockThreadSleeper);
		}

		[TestCleanup]
		public void TearDown()
		{
			_mockPoster.VerifyAllExpectations();
			_mockThreadSleeper.VerifyAllExpectations();
		}

		[TestMethod]
		public void Valid_Response_Verify_Passthrough()
		{
			_mockPoster.Expect(x => x.PostData("a", "b")).Return("c");
			Assert.AreEqual("c", _rpu.PostData("a", "b"));
		}

		[TestMethod]
		public void Error_Response_Verify_Second_Try()
		{
			_mockPoster.Expect(x => x.PostData("a", "b")).Return("-1").Repeat.Once();
			_mockPoster.Expect(x => x.PostData("a", "b")).Return("c");
			Assert.AreEqual("c", _rpu.PostData("a", "b"));
		}

		[TestMethod]
		public void Error_Blank_Response_Verify_Second_Try()
		{
            _mockPoster.Expect(x => x.PostData("a", "b")).Return("").Repeat.Once(); ;
			_mockPoster.Expect(x => x.PostData("a", "b")).Return("c");
			Assert.AreEqual("c", _rpu.PostData("a", "b"));
		}

		[TestMethod]
		public void Multiple_Error_Responses_Verify_5_Retries()
		{
			_mockPoster.Expect(x => x.PostData("a", "b")).Return("-1").Repeat.Times(5);

			//This verifies that the sleep method is called 4 times
			_mockThreadSleeper.Expect(x => x.Sleep(TimeSpan.FromSeconds(1.0))).Repeat.Times(4);

			Assert.AreEqual("-1", _rpu.PostData("a", "b"));
		}
	}
}

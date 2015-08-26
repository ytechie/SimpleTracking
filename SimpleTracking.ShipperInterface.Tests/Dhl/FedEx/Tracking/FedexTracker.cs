using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SimpleTracking.ShipperInterface.FedexTrackWebService;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using SimpleTracking.ShipperInterface.Util;

namespace SimpleTracking.ShipperInterface.FedEx.Tracking
{
	[TestClass]
	public class FedexTracker_Tester
	{
        private MockRepository mocks;
        private ITrackService _trackService;
        private FedexTracker _tracker;

		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			mocks = new MockRepository();
            _trackService = MockRepository.GenerateMock<ITrackService>();

			//_tracker = new FedexTracker(_trackService, "key", "password", "accountNumber", "meterNumber", true);
		}

		#endregion

        [TestMethod]
        public void Set_Production_Tracking_URL()
        {
            _trackService.Expect(x => x.Url).SetPropertyWithArgument("https://ws.fedex.com/web-services");

            _tracker = new FedexTracker(_trackService, "key", "password", "accountNumber", "meterNumber", true);
            var td = _tracker.GetTrackingData("9274890100130166388000");

            _trackService.VerifyAllExpectations();
        }

        [TestMethod]
        public void VerifyRequestProperties()
        {
            _tracker = new FedexTracker(_trackService, "key", "password", "accountNumber", "meterNumber", true);

            _trackService.Expect(x => x.track(Arg<TrackRequest>.Matches(y =>
                        y.IncludeDetailedScans == true
                        && y.IncludeDetailedScansSpecified == true
                        && y.WebAuthenticationDetail.UserCredential.Key == "key"
                        && y.WebAuthenticationDetail.UserCredential.Password == "password"
                        && y.ClientDetail.AccountNumber == "accountNumber"
                        && y.ClientDetail.MeterNumber == "meterNumber"
                        && y.PackageIdentifier.Value == "9274890100130166388000"
                        && y.PackageIdentifier.Type == FedexTrackWebService.TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG
                    )));

            var td = _tracker.GetTrackingData("9274890100130166388000");

            _trackService.VerifyAllExpectations();
        }

        //[TestMethod]
        //public void ProcessBasicResponse()
        //{
        //    _tracker = new FedexTracker(_trackService, "key", "password", "accountNumber", "meterNumber", true);

        //    _trackService.Expect(x => x.track(Arg<TrackRequest>.Is.Anything)).Return(null);

        //    var td = _tracker.GetTrackingData("9274890100130166388000");

        //    //Asserts
        //}

        #region Tracking Number Checks

        [TestMethod]
		public void Check_Fedex_Tracking_Number_With_10_Checksum()
		{
			Assert.IsTrue(FedexTracker.IsFedExExpress("966645788660"));
		}

		[TestMethod]
		public void Check_Fedex_Tracking_Number_With_Invalid_Character()
		{
			Assert.IsFalse(FedexTracker.IsFedExExpress("01234a678984"));
		}

		[TestMethod]
		public void Check_Fedex_Tracking_Number_With_Invalid_Check_Digit()
		{
			Assert.IsFalse(FedexTracker.IsFedExExpress("012345678984"));
		}

		[TestMethod]
		public void Check_Fedex_Tracking_Number_With_Invalid_Length()
		{
			Assert.IsFalse(FedexTracker.IsFedExExpress("012"));
		}

		[TestMethod]
		public void Check_Valid_Fedex_Ground_Number()
		{
			Assert.IsTrue(FedexTracker.IsFedExGround("987654312345672"));
		}

		[TestMethod]
		public void Check_Valid_Fedex_Ground_Number2()
		{
			Assert.IsTrue(FedexTracker.IsFedExGround("729445719235295"));
		}

		[TestMethod]
		public void Check_Valid_Fedex_Ground_Number3()
		{
			Assert.IsTrue(FedexTracker.IsFedExGround("230708973660298"));
		}

		[TestMethod]
		public void Check_Valid_Fedex_Tracking_Number()
		{
			Assert.IsTrue(FedexTracker.IsFedExExpress("012345678983"));
		}

		[TestMethod]
		public void Check_Valid_Fedex_Tracking_Number2()
		{
			Assert.IsTrue(FedexTracker.IsFedExExpress("864737425688"));
		}

        [TestMethod]
        public void Check_Valid_FedExSmartPost()
        {
            Assert.IsTrue(FedexTracker.IsFedExSmartPost("9274890100130166388000"));
        }

		[TestMethod]
		public void Chec_Ups_Number_Doesnt_Match_Fedex_Format()
		{
			Assert.IsFalse(FedexTracker.IsFedExExpress("1Z039AF20326069009") || FedexTracker.IsFedExGround("1Z039AF20326069009"));
		}

        #endregion
	}
}
using System;
namespace SimpleTracking.ShipperInterface.FedEx
{
    public interface ITrackService
    {
        SimpleTracking.ShipperInterface.FedexTrackWebService.TrackReply track(SimpleTracking.ShipperInterface.FedexTrackWebService.TrackRequest TrackRequest);
        string Url { get; set; }
    }
}

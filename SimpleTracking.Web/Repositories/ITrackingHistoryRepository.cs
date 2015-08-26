using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.Web.Repositories
{
    public interface ITrackingHistoryRepository
    {
        void SaveTrackingNumber(string trackingNumber);
        void SaveTrackingData(string trackingNumber, TrackingData trackingData);
    }
}

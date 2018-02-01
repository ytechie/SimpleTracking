using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SimpleTracking.ShipperInterface.Tracking
{
    public class LoggingTracker : ITracker
    {
        private ITracker _upstreamTracker;

        public LoggingTracker(ITracker upstreamTracker)
        {
            _upstreamTracker = upstreamTracker;
        }

        public TrackingData GetTrackingData(string trackingNumber)
        {
            var td = _upstreamTracker.GetTrackingData(trackingNumber);

            try
            {
                StoreTrackingData(trackingNumber, td);
            }
            catch (Exception) { }

            return td;
        }

        private void StoreTrackingData(string trackingNumber, TrackingData trackingData)
        {
            //If we don't have an internet connection (offline dev), don't use storage
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return;

            var trackingNumberCacheConnectionString = WebConfigurationManager.AppSettings["TrackingNumberCacheConnectionString"];

            //Store some tracking numbers to test with
            var storageAccount = CloudStorageAccount.Parse(trackingNumberCacheConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("trackrequests");

            //This only had to run 1 time
            //table.CreateIfNotExists();

            var data = new TrackingDataEntity(trackingNumber, trackingData);
            
            var insert = TableOperation.InsertOrReplace(data);
            table.BeginExecute(insert, new AsyncCallback(InsertComplete), table);
        }

        private void InsertComplete(IAsyncResult result)
        {
            try
            {
                var table = result.AsyncState as CloudTable;
                if (table != null)
                    table.EndExecute(result);
            }
            catch (Exception ex)
            {
                //In production, I don't want this to crash since it's just logging
#if DEBUG
                throw ex;
#endif
            }
        }
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.DataAccess
{
    public class TrackingDataEntity : TableEntity
    {
        public string TrackingNumber { get; private set; }
        public string TrackingDataJson { get; set; }

        public TrackingDataEntity(string trackingNumber, TrackingData trackingData)
        {
            PartitionKey = trackingNumber;
            RowKey = DateTime.UtcNow.ToString("O");

            TrackingNumber = trackingNumber;

            //What is the performance impact here?
            TrackingDataJson = JsonConvert.SerializeObject(trackingData);
        }
    }
}

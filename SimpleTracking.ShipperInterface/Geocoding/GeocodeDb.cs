using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SimpleTracking.ShipperInterface.Geocoding
{
    public class GeocodeDb : IGeocodeDb
    {
        private readonly CloudTable _table;

        private readonly Queue<CityRecord> _pending;

        private int _executing = 0;

        private const int ConcurrencyMax = 300;

        public GeocodeDb(string geocodeDbConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(geocodeDbConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference("cities");

            //This only had to run 1 time
            _table.CreateIfNotExists();

            _pending = new Queue<CityRecord>();
        }

        public void InsertCities(IEnumerable<CityRecord> cities)
        {
            foreach (var city in cities)
            {
                InsertCity(city);
            }
        }

        private void InsertCity(CityRecord city)
        {
            if (city == null)
                return; //not sure when this gets hit

            lock (_pending)
            {
                if (_executing >= ConcurrencyMax)
                {

                    _pending.Enqueue(city);
                    return;
                }
            }

            var insert = TableOperation.InsertOrReplace(city);
            _table.BeginExecute(insert, InsertComplete, _table);
            lock (_pending)
            {
                _executing++;
            }
        }

        private void InsertComplete(IAsyncResult result)
        {
            _table.EndExecute(result);

            //Get the next item to insert
            lock (_pending)
            {
                _executing--;

                if(_pending.Count % 100 == 0)
                    Debug.WriteLine("{0} records pending", _pending.Count);

                if(_executing == 0 && _pending.Count == 0)
                    Debug.WriteLine("No pending city inserts");

                if (_pending.Count > 0)
                    InsertCity(_pending.Dequeue());
            }
        }

        public CityRecord GetCity(string city, string state)
        {
            var c = city.ToUpper();
            var s = state.ToUpper();

            var records = _table.CreateQuery<CityRecord>()
                .Where(x => x.PartitionKey == c && x.RowKey == s)
                .ToList();

            return records.FirstOrDefault();
        }
    }
}

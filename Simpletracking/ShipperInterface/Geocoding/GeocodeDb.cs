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
        public CityRecord GetCity(string city, string state)
        {
            var rec = new CityRecord
            {
                City = city,
                State = state,
                Zip = ""
            };

            return rec;
        }
    }
}

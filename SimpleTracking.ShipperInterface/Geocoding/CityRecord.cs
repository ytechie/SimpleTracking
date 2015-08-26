using Microsoft.WindowsAzure.Storage.Table;

namespace SimpleTracking.ShipperInterface.Geocoding
{
    public class CityRecord : TableEntity
    {
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string County { get; set; }

        public CityRecord()
        {
        }

        public CityRecord(string serialized)
        {
            var parts = serialized.Split(",".ToCharArray());
            Zip = parts[0];
            City = parts[1];
            State = parts[2];
            Latitude = double.Parse(parts[3]);
            Longitude = -double.Parse(parts[4]);
            County = parts[5];

            PartitionKey = City;
            RowKey = State;
        }
    }
}

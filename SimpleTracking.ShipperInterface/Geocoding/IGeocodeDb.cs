using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.Geocoding
{
    public interface IGeocodeDb
    {
        void InsertCities(IEnumerable<CityRecord> cities);
        CityRecord GetCity(string city, string state);
    }
}

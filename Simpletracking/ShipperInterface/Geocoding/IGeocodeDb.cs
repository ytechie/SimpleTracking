namespace SimpleTracking.ShipperInterface.Geocoding
{
    public interface IGeocodeDb
    {
        CityRecord GetCity(string city, string state);
    }
}

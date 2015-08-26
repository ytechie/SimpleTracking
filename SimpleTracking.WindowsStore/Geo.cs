using System;
using Windows.Devices.Geolocation;
using Bing.Maps;

namespace SimpleTracking.WindowsStore
{
    public static class Geo
    {
        public static double GetDistanceTo(this Geocoordinate other, Location first)
        {
            if (double.IsNaN(first.Latitude) || double.IsNaN(first.Longitude) || double.IsNaN(other.Point.Position.Latitude) || double.IsNaN(other.Point.Position.Longitude))
            {
                throw new ArgumentException("Argument_LatitudeOrLongitudeIsNotANumber");
            }
            else
            {
                double latitude = first.Latitude * 0.0174532925199433;
                double longitude = first.Longitude * 0.0174532925199433;
                double num = other.Point.Position.Latitude * 0.0174532925199433;
                double longitude1 = other.Point.Position.Longitude * 0.0174532925199433;
                double num1 = longitude1 - longitude;
                double num2 = num - latitude;
                double num3 = Math.Pow(Math.Sin(num2 / 2), 2) + Math.Cos(latitude) * Math.Cos(num) * Math.Pow(Math.Sin(num1 / 2), 2);
                double num4 = 2 * Math.Atan2(Math.Sqrt(num3), Math.Sqrt(1 - num3));
                double num5 = 6376500 * num4;
                return num5;
            }
        }
    }
}

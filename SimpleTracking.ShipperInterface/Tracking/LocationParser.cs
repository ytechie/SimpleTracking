using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.Tracking
{
    public class AddressLocation
    {
        public string City { get; set; }
        public string State { get; set; }
    }

    public class LocationParser
    {
        public static AddressLocation ParseLocation(string locationString)
        {
            var location = new AddressLocation { City = "", State = "" };

            if (string.IsNullOrWhiteSpace(locationString))
                return location;

            var parts = locationString.Split(",".ToCharArray());
            
            switch (parts.Length)
            {
                case 0:
                    break;
                case 1:
                    location.State = parts[0].Trim();
                    break;
                case 2:
                    location.City = parts[0].Trim();
                    location.State = parts[1].Trim();
                    break;
                case 3:
                    location.City = parts[0].Trim();
                    location.State = parts[1].Trim();
                    break;
            }

            return location;
        }
    }
}

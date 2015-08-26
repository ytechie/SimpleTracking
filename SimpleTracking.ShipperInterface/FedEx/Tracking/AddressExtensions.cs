using SimpleTracking.ShipperInterface.FedexTrackWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.FedEx.Tracking
{
    public static class AddressExtensions
    {
        public static string GetFriendlyAddressString(this Address address)
        {
            string serialized = "";
            
            serialized += address.City;

            if (!string.IsNullOrWhiteSpace(address.StateOrProvinceCode))
                serialized += ", " + address.StateOrProvinceCode;

            if (address.CountryCode != "US" && !string.IsNullOrWhiteSpace(address.CountryCode))
                serialized += ", " + address.CountryCode;

            return serialized;            
        }
    }
}

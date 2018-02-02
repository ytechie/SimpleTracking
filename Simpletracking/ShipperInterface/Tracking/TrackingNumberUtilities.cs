using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.Tracking
{
    public class TrackingNumberUtilities
    {
        /// <summary>
        ///     Cleans a tracking number, removing bad characters. This should not be used in a
        ///     tracker, it should be used from the standardizer tracker.
        /// </summary>
        /// <param name="trackingNumber"></param>
        /// <param name="allowLetters"></param>
        /// <returns></returns>
        public static string CleanTrackingNumber(string trackingNumber, bool allowLetters)
        {
            if (trackingNumber == null)
                return "";

            var sb = new StringBuilder();

            foreach (var c in trackingNumber)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
using System.Text;

namespace SimpleTracking.ShipperInterface.Common
{
	/// <summary>
	///		Methods for working with a location that has a city, state, and country code.
	/// </summary>
	public static class Location
	{
		/// <summary>
		///		Combines a city state and country code into a single string.
		/// </summary>
		/// <param name="city">
		///		The name of the city, this can be NULL.
		/// </param>
		/// <param name="state">
		///		The name of the state, this can be NULL.
		/// </param>
		/// <param name="countryCode">
		///		The 2 digit country code, this can be NULL.
		/// </param>
		/// <returns>
		///		A string that combines the city, state, and country code in a
		///		friendly user format.
		/// </returns>
		public static string GetLocationString(string city, string state, string countryCode)
		{
			bool firstComponent = true;
			var s = new StringBuilder();

			if (city != null)
			{
				s.Append(city);
				firstComponent = false;
			}
			if (state != null)
			{
				if (!firstComponent)
					s.Append(", ");

				s.Append(state);
				firstComponent = false;
			}
			if (countryCode != null)
			{
				if (!firstComponent)
					s.Append(", ");

				s.Append(countryCode);
			}

			return s.ToString();
		}
	}
}
using System;

namespace SimpleTracking.ShipperInterface.ClientServerShared
{
	/// <summary>
	///     A unified format to store package activity data. An instance
	///     of this class represents a step that a package took to get to a
	///     destination.
	/// </summary>
	/// <remarks>
	///     This is often referred to as a scan.
	/// </remarks>
	public class Activity
	{
	    /// <summary>
		///		Gets or sets a human read-able string that contains
		///		the location that the package was scanned at.
		/// </summary>
		public string LocationDescription { get; set; }

		/// <summary>
		///		A short description of what even this activity
		///		constitutes. For example, this could say that the
		///		package was scanned or delivered.
		/// </summary>
		public string ShortDescription { get; set; }

		/// <summary>
		///		The <see cref="DateTime"/> that the activity occurred, as
		///		reported by the shipper.
		/// </summary>
		public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        /// <summary>
        ///     Created, delivered, etc.
        /// </summary>
        public ShipmentStage? Stage { get; set; }
	}
}
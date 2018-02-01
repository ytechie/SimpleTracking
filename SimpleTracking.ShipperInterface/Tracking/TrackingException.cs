namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		Represents an exception that occurs while trying
	///		to retreive the tracking information from a tracking
	///		system.
	/// </summary>
	public class TrackingException : ShipperInterfaceException
	{
		/// <summary>
		///		Creates a new instance of the <see cref="TrackingException"/>.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number that was attempting to be tracked.
		/// </param>
		public TrackingException(string trackingNumber)
		{
			TrackingNumber = trackingNumber;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="TrackingException"/>.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number that was attempting to be tracked.
		/// </param>
		/// <param name="message">
		///		A message with a description of why the exception was raised.
		/// </param>
		public TrackingException(string trackingNumber, string message) : base(message)
		{
			TrackingNumber = trackingNumber;
		}

		/// <summary>
		///		Gets or sets the tracking number associated with
		///		this exception.
		/// </summary>
		public string TrackingNumber { get; set; }
	}
}
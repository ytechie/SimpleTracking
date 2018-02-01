using System;

namespace SimpleTracking.ShipperInterface
{
	/// <summary>
	///		The base class for all exceptions created by
	///		the shipper interface DLL.
	/// </summary>
	public class ShipperInterfaceException : Exception
	{
		/// <summary>
		///		Creates a new instance of the <see cref="ShipperInterfaceException"/>.
		/// </summary>
		public ShipperInterfaceException() : base("")
		{
		}

		/// <summary>
		///		Creates a new instance of the <see cref="ShipperInterfaceException"/>.
		/// </summary>
		/// <param name="message">
		///		A message describing the cause of the exception.
		/// </param>
		public ShipperInterfaceException(string message) : base(message)
		{
		}

		/// <summary>
		///		Creates a new instance of the <see cref="ShipperInterfaceException"/>.
		/// </summary>
		/// <param name="message">
		///		A message describing the cause of the exception.
		/// </param>
		/// <param name="innerException">
		///		The exception that is being wrapped by this exception.
		/// </param>
		public ShipperInterfaceException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}
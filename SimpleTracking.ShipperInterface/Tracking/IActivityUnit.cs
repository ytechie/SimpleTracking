using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		This interface gives us a common way to translate
	///		multiple activity types from different shippers into
	///		our common <see cref="Activity"/> formats.
	/// </summary>
	public interface IActivityUnit
	{
		/// <summary>
		///		Converts this activity unit into an activity.
		/// </summary>
		/// <returns>
		///		An <see cref="Activity"/> that represents this
		///		activity unit.
		/// </returns>
		Activity GetActivity();
	}
}
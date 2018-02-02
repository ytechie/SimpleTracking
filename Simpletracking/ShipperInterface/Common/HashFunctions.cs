using System;

namespace SimpleTracking.ShipperInterface.Common
{
	/// <summary>
	/// Contains functions that implement different Hash Algorithms.
	/// </summary>
	public static class HashFunctions
	{
		/// <summary>
		/// Implements a rotative hash function using the four supplied numbers.
		/// For the hash function to be most affective, four prime numbers should be used.
		/// </summary>
		/// <param name="str">The string to hash.</param>
		/// <param name="p1">First number (preferably prime)</param>
		/// <param name="p2">Second number (preferably prime)</param>
		/// <param name="p3">Third number (preferably prime)</param>
		/// <param name="p4">Fourth number (preferably prime)</param>
		/// <returns></returns>
		public static UInt32 APHash(string str, Int32 p1, Int32 p2, Int32 p3, Int32 p4)
		{
			UInt32 hash = 0;

			for (UInt32 i = 0; i < str.Length; i++)
			{
				hash += ((i & 1) == 0)
				        	? ((hash << p1) ^ str.ToCharArray()[i] ^ (hash >> p2))
				        	:
				        		(~((hash << p3) ^ str.ToCharArray()[i] ^ (hash >> p4)));
			}

			return (hash & 0x7FFFFFFF);
		}
	}
}
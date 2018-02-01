using System;

namespace SimpleTracking.ShipperInterface.Common
{
	/// <summary>
	///		Contains algorithms for common check digit algorithms.
	/// </summary>
	public static class CheckDigits
	{
		/// <summary>
		///		Checks if the string has a valid MOD 10 check digit.
		/// </summary>
		/// <param name="checkString">
		///		The string to check for a valid MOD 10 check digit.
		/// </param>
		/// <returns>
		///		True if the string has a valid check digit, otherwise false.
		/// </returns>
		public static bool CheckMod10CheckDigit(string checkString)
		{
			char[] chars = checkString.ToCharArray();
			Array.Reverse(chars);
			int num1 = 0;
			for (int i = 1; i < chars.Length; i += 2)
				num1 += (chars[i] - 48);

			num1 *= 3;

			int num2 = 0;
			for (int i = 2; i < chars.Length; i += 2)
				num2 += (chars[i] - 48);

			num1 += num2;

			int checkDigit = 10 - (num1%10);

			return checkString.EndsWith(checkDigit.ToString());
		}
	}
}
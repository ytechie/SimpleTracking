using System;
using NUnit.Framework;

namespace YTech.ShipperInterface.Dhl
{
	[TestFixture]
	public class FormatConversions_Tester
	{
		[Test]
		public void ParseDhlDate()
		{
			DateTime result;

			result = FormatConversions.ParseDhlDate("2002/06/24T16:23:27");
			Assert.AreEqual(new DateTime(2002, 06, 24, 16, 23, 27), result);
		}
	}
}
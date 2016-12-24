using System;

namespace Mxp.Core.Utils
{
	public static class DateTimeExtentions
	{
		public static int Timestamp (this DateTime dateTime) {
			return (int) (dateTime.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
		}
	}
}
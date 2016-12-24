using System;
using Foundation;

namespace Mxp.iOS
{
	public static class DateTimeExtensions
	{
		public static NSDate DateTimeToNSDate(this DateTime date)
		{
			if (date.Kind == DateTimeKind.Unspecified)
				date = DateTime.SpecifyKind (date, DateTimeKind.Local/* DateTimeKind.Local or DateTimeKind.Utc, this depends on each app */);
					return (NSDate) date;
		}

		public static DateTime NSDateToDateTime(NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime( 
				new DateTime(2001, 1, 1, 0, 0, 0) );
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}
		
	}
}


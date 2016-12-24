using System;
using System.Text;
using System.Globalization;
using Mxp.Core.Extensions;

namespace Mxp.Utils
{
	public static class StringExtensions
	{
		public static int? ToInt (this string str) {
			if (String.IsNullOrWhiteSpace (str))
				return null;

			int integer;
			Int32.TryParse (str, out integer);
			return integer;
		}

		public static double? ToDouble (this string str) {
			if (String.IsNullOrWhiteSpace (str))
				return null;

			double value;
			Double.TryParse (str, out value);
			return value;
		}

		public static bool ToBool(this string str) {
			if (String.IsNullOrWhiteSpace (str))
				return false;

			str = str.Trim ().ToLowerInvariant ();

			return str.Equals ("show") || str.Equals ("true") || str.Equals ("t") || str.Equals ("yes") || str.Equals ("y");
		}

		public static DateTime? ToDateTime (this string str, string pattern = null) {
			if (String.IsNullOrWhiteSpace (str))
				return null;

			DateTime date;

			if (String.IsNullOrEmpty (pattern))
				DateTime.TryParse (str, null, DateTimeStyles.None, out date);
			else
				DateTime.TryParseExact (str, pattern, null, DateTimeStyles.None, out date);
			
			return date;
		}

		public static TimeSpan? ToTimeSpan (this string str, string pattern = null) {
			if (String.IsNullOrWhiteSpace (str))
				return null;

			TimeSpan time;

			if (String.IsNullOrEmpty (pattern))
				TimeSpan.TryParse (str, out time);
			else
				TimeSpan.TryParseExact (str, pattern, null, out time);

			return time;
		}

		public static string ToTitleCase (this string str) {
			string result = str;
			if (!string.IsNullOrEmpty(str)) {
				string[] words = str.Split(' ');
				for (int index = 0; index < words.Length; index++) {
					var s = words[index];
					if (s.Length > 0)
						words[index] = s[0].ToString().ToUpper() + s.Substring(1);
				}
				result = string.Join(" ", words);
			}

			return result;
		}

		public static string Capitalize(this string str) {
			if (String.IsNullOrEmpty (str))
				return String.Empty;

			if (str.Length > 1)
				return char.ToUpper (str[0]) + str.Substring (1);

			return str.ToUpper ();
		}

		public static T ToEnum<T> (this string value) {
			return (T) Enum.Parse (typeof(T), value, true);
		}

		public static T ToEnum<T> (this string value, T defaultValue) where T : struct {
			if (string.IsNullOrEmpty (value))
				return defaultValue;

			T result;
			return Enum.TryParse<T> (value, true, out result) ? result : defaultValue;
		}

		public static bool Contains(this string source, string value, StringComparison comparison) {
			return source != null && value != null && source.IndexOf (value, comparison) >= 0;
		}

		public static bool IsNullOrDefaultInt (this string value) {
			return value == null || (value.IsInt () && Convert.ToInt32 (value).IsNullOrDefault ());
		}

		public static bool IsInt (this string str) {
			int id;
			return Int32.TryParse (str, out id);
		}
	}
}
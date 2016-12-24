using System;
using System.Collections.Generic;

namespace Mxp.Core.Extensions
{
	public static class GenericExtensions
	{
		public static bool IsDefault<T> (this T value) {
			return EqualityComparer<T>.Default.Equals (value, default (T));
		}

		public static bool IsNullOrDefault<T> (this T value) {
			return value == null || value.IsDefault ();
		}
	}
}
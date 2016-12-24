using System;
using System.Collections.Generic;
using System.Collections;
using Mxp.Core.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using Mxp.Core.Business;

namespace Mxp.Core.Utils
{
	public static class IEnumerableExtensions
	{
		public static E SelectSingle<T, E> (this IEnumerable<T> enumerable, Func<T, E> action) {
			foreach(T item in enumerable) {
				E @object = action (item);
				if (@object != null)
					return @object;
			}

			return default (E);
		}

		public static void ForEach<T> (this IEnumerable<T> enumerable, Action<T> action) {
			foreach(T item in enumerable) action (item);
		}

		public static void ForEach<T> (this IEnumerable<T> enumerable, Action<T, int> action) {
			int index = 0;
			foreach(T item in enumerable) action (item, index++);
		}

		public static IEnumerable<T> DeepCopy<T> (this IEnumerable<T> enumerable) where T : ICloneable {
			return new List<T> (enumerable.Select (x => x.Clone ()).Cast<T> ());
		}

		public static bool IsEmpty<T> (this IEnumerable<T> enumerable) {
			return !enumerable.Any ();
		}
	}
}
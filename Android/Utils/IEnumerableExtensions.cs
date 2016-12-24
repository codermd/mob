using System;
using System.Collections.Generic;
using System.Collections;
using Mxp.Core.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using Mxp.Core.Business;
using Mxp.Droid.Helpers;

namespace Mxp.Droid.Utils
{
	public static class IEnumerableExtensions
	{
		public static List<WrappedObject> AsWrappedObject<T> (this IEnumerable<T> enumerable) {
			return enumerable.Select (x => new WrappedObject (x)).ToList ();
		}
	}
}
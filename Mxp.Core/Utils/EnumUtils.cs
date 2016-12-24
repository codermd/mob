using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using Mxp.Core.Business;

namespace Mxp.Core.Utils
{
	public static class EnumUtils
	{
		public static T GetWeakestEnum<T>(IEnumerable items, string propertyName) {
			int policy = Enum.GetValues(typeof(T)).Cast<int>().Max();
			foreach (object item in items) {
				int itemPolicy = (int) item.GetType ().GetRuntimeProperty(propertyName).GetValue(item, null);
//				Debug.WriteLine ((T) (object)itemPolicy);
				if (itemPolicy == 0) {
					return (T) (object)itemPolicy;
				} else if (itemPolicy < policy) {
					policy = itemPolicy;
				}
			}

			return (T) (object)policy;
		}
	}
}
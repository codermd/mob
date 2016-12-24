using System;

namespace Mxp.Droid.Utils
{		
	public static class ObjectTypeExtensions
	{
		public static T Cast<T>(this Java.Lang.Object obj) where T : class {
			var propertyInfo = obj.GetType().GetProperty("Instance");
			return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
		}

//		public static Java.Lang.Object Cast<T> (this T obj) where T : class {
//			return Java.Interop.JavaConvert.ToJavaObject<T> (obj).JavaCast<Java.Lang.Object> ();
//		}
	}
}
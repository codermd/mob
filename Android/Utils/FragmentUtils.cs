using System;

namespace Mxp.Droid.Utils
{
	public static class FragmentUtils
	{
		public static string FragmentJavaName (Type fragmentType) {
			var namespaceText = fragmentType.Namespace ?? "";
			if (namespaceText.Length > 0)
				namespaceText = namespaceText.ToLowerInvariant() + ".";
			return namespaceText + fragmentType.Name;
		}
	}
}
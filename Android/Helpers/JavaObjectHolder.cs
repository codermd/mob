using System;

namespace Mxp.Droid.Helpers
{
	public class JavaObjectHolder<T> : Java.Lang.Object where T : class
	{
		public T Instance { get; private set; }

		public JavaObjectHolder (T instance) {
			this.Instance = instance;
		}
	}
}
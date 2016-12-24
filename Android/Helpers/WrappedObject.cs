using System;

namespace Mxp.Droid.Helpers
{
	public class WrappedObject
	{
		private object @object;

		public WrappedObject (object @object) {
			this.@object = @object;
		}

		public T GetInstance<T> () {
			return (T) this.@object;
		}

		public bool IsInstanceOf<T> () {
			return this.@object is T;
		}
	}
}
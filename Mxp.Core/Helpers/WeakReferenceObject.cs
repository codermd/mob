using System;
using Mxp.Core.Business;

namespace Mxp.Core.Helpers
{
	public class WeakReferenceObject<T> where T : class
	{
		private WeakReference<T> value;
		public T Value {
			get {
				T target;

				this.value.TryGetTarget (out target);

				return target;
			}
			private set {
				this.value = new WeakReference<T> (value);
			}
		}

		public WeakReferenceObject (T entity) {
			this.Value = entity;
		}
	}
}
using System;

namespace Mxp.Core.Helpers
{
	public class EventArgsObject<T> : EventArgs
	{
		public T Object { get; private set; }

		public EventArgsObject (T obj) {
			this.Object = obj;
		}
	}
}
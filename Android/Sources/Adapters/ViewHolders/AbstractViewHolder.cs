using System;
using Mxp.Core.Business;

namespace Mxp.Droid.Helpers
{
	public abstract class ViewHolder<E> : Java.Lang.Object where E : class {
		public abstract void BindView (E entity);
	}
}
using System;
using Mxp.Core.Business;

namespace Mxp.Droid
{
	public interface IReportListener
	{
		Report GetReport ();

		void RefreshIntentExtras ();
	}
}
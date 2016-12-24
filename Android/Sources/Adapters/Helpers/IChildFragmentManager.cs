using System;
using Android.Support.V4.App;

namespace Mxp.Droid.Helpers
{	
	public interface IChildFragmentManager
	{
		FragmentManager GetChildFragmentManager ();
	}
}
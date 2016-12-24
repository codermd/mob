using System;
using Mxp.Core;
using Android.Content.PM;
using Android.App;
using Android.Support.V7.AppCompat;
using Mxp.Droid.Helpers;
using Android.Content;

[assembly: Xamarin.Forms.Dependency (typeof (ServiceAppVersion))]

namespace Mxp.Droid.Helpers
{
	public class ServiceAppVersion : Java.Lang.Object, IServiceAppVersion
	{
		public ServiceAppVersion () {}

		public string AppVersion () {
			Context context = MxpApplication.Context;
			return context.PackageManager.GetPackageInfo (context.PackageName, 0).VersionName;
		}
	}
}
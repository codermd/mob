using System;
using Mxp.iOS;
using Foundation;
using Mxp.Core;

[assembly: Xamarin.Forms.Dependency (typeof (ServiceAppVersion))]

namespace Mxp.iOS
{
	public class ServiceAppVersion : IServiceAppVersion
	{
		public ServiceAppVersion () {}

		public string AppVersion () {
			return NSBundle.MainBundle.ObjectForInfoDictionary ("CFBundleShortVersionString").ToString ();
		}
	}
}
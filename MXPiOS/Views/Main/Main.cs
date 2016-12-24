using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using System.Collections.ObjectModel;
using System.Diagnostics;
using Mxp.Core.Services;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class Application
	{
		public static void Main (string[] args) {
			AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {};

			try  {
				new ApparenceConfiguration ();
				UIApplication.Main (args, null, "AppDelegate");
			} catch (Exception error) {
				LoggedUser.Instance.TrackContext.Exception = error;
				LoggedUser.Instance.ResetData ();
				Console.Write (error.Message);
			}
		}
	}
}
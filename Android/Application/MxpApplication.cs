using System;
using Android.App;
using Java.Lang;
using Android.Runtime;
using Android.Support.V7.App;
using Mxp.Droid.Utils;
using Mxp.Core.Services;
using Mxp.Droid.Fragments;
using Mxp.Core.Business;
using Android.Content;
using System.Reflection;
using Android.Support.V4.App;
using Java.Interop;
using Java.IO;
using Console = System.Console;
using File = Java.IO.File;

namespace Mxp.Droid
{
	[Application]
	public class MxpApplication : Application
	{
		private BaseActivity _currentActivity;
		public BaseActivity mCurrentActivity { 
			get {
				return this._currentActivity;
			} 
			set {
				this._currentActivity = value;

				if (value != null)
					LoggedUser.Instance.TrackContext.AddViews (this._currentActivity.GetType ().ToString ());
			}
		}

		// Required to work
		protected MxpApplication (IntPtr javaReference, JniHandleOwnership transfer)
			: base (javaReference, transfer) {

		}

		public override void OnCreate () {
			base.OnCreate ();

//			Thread.DefaultUncaughtExceptionHandler = new CustomExHandler ();
			AndroidEnvironment.UnhandledExceptionRaiser += (object sender, RaiseThrowableEventArgs e) => {
				typeof (System.Exception).GetField ("stack_trace", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(e.Exception, null);
				this.OnExceptionCaught (e.Exception);
				e.Handled = true;
			};

			AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
				typeof (System.Exception).GetField ("stack_trace", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(e.ExceptionObject, null);
				this.OnExceptionCaught (e.ExceptionObject as System.Exception);
			};
		}

		private void OnExceptionCaught (System.Exception ex) {
			this.mCurrentActivity.InvokeActionAsync (
				() =>  SystemService.Instance.LogExceptionAsync (ex),
				() => {
					Android.Support.V4.App.DialogFragment errorDialogFragment = BaseDialogFragment.NewInstance (
						this.mCurrentActivity,
						BaseActivity.mExceptionDialogRequestCode,
						BaseDialogFragment.DialogTypeEnum.ExceptionDialog,
						#if DEBUG
						ex.Message + "\n\n"
						+ ex.StackTrace
						#else
						"An error occured on the app when processing your request.\n\n" +
						"Technical support has automatically already been notified. The problem will be solved as soon as possible, in the worst case in the next update.\n\n" +
						"We appologize for the inconvenience and thank you for your understanding"
						#endif
					);
					this.mCurrentActivity.onClickHandler += (object resender, EventArgs re) => {
						LoggedUser.Instance.ResetData ();
//						Android.OS.Process.KillProcess (Android.OS.Process.MyPid ());
						ActivityCompat.FinishAffinity (this.mCurrentActivity);
						System.Environment.Exit (0);
					};
					errorDialogFragment.Show (this.mCurrentActivity.SupportFragmentManager, null);
				}
			);
		}

		private class CustomExHandler : Java.Lang.Object, Thread.IUncaughtExceptionHandler {
			public static Thread.IUncaughtExceptionHandler mDefaultHandler = Thread.DefaultUncaughtExceptionHandler;

			public void UncaughtException (Thread thread, Throwable ex) {
				mDefaultHandler.UncaughtException (thread, ex);
			}
		}

        [Export("CopyImageBackdoor")]
	    public string CopyImageBackdoor()
        {
            try
            {
                var ins = Assets.Open("UITestImage.png");
                var buffersize = 1024;
                var PictureFile =
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                var outFileName = PictureFile.AbsolutePath + @"/UITestImage.png";

                var outs = Context.OpenFileOutput("UITestImage.png",FileCreationMode.Private);
                
                // Open the empty db as the output stream
                //var outs = new FileOutputStream();

                // transfer bytes from the inputfile to the outputfile
                var buffer = new byte[buffersize];
                int length;
                while ((length = ins.Read(buffer, 0, buffersize)) > 0)
                {
                    outs.Write(buffer, 0, length);
                }
                // Close the streams
                outs.Flush();
                outs.Close();
                ins.Close();

                return outFileName;
            }
            catch (System.Exception e)
            {
                return "Exception: " + e.Message;
            }
        }
	}
}
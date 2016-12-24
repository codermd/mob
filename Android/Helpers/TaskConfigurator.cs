using System;
using System.Threading.Tasks;
using Android.Support.V4.App;
using Mxp.Droid.Fragments;

namespace Mxp.Droid.Helpers {
	public class TaskConfigurator {
		public FragmentActivity TargetActivity { get; set; }
		public Fragment TargetFragment { get; set; }

		public bool CanShowErrorDialog { get; set; } = true;
		public bool CanFinishActivity { get; set; }
		public bool WithProgress { get; set; }

		public Action<string> CatchCallback;
		public Action FinallyCallback;
		public Action<object> TypedFinallyCallback;

		public static TaskConfigurator Create () {
			return new TaskConfigurator ().SetCanShowErrorDialog (false);
		}

		public static TaskConfigurator Create (FragmentActivity activity) {
			return new TaskConfigurator (activity);
		}

		public static TaskConfigurator Create (Fragment fragment) {
			return new TaskConfigurator (fragment);
		}

		private TaskConfigurator () {

		}

		private TaskConfigurator (FragmentActivity activity) : this () {
			this.TargetActivity = activity;
		}

		private TaskConfigurator (Fragment fragment) : this (fragment.Activity) {
			this.TargetFragment = fragment;
		}

		public TaskConfigurator Catch (Action<string> callback) {
			this.CatchCallback = callback;
			return this;
		}

		public TaskConfigurator Finally (Action callback) {
			this.FinallyCallback = callback;
			return this;
		}

		public TaskConfigurator Finally<T> (Action<T> callback) {
			this.TypedFinallyCallback = new Action<object> (o => callback((T)o));
			return this;
		}

		public TaskConfigurator SetCanFinishActivity (bool finish) {
			this.CanFinishActivity = finish;
			return this;
		}

		public TaskConfigurator SetCanShowErrorDialog (bool show) {
			this.CanShowErrorDialog = show;
			return this;
		}

		public TaskConfigurator SetWithProgress (bool progress) {
			this.WithProgress = progress;
			return this;
		}

		public void Validate () {
			if (this.CanShowErrorDialog) {
				if (this.TargetFragment != null && !(this.TargetFragment is BaseDialogFragment.IDialogClickListener))
					throw new InvalidCastException ("Calling fragment " + this.TargetFragment.Class.SimpleName + " must implement IDialogClickListener interface");
				else if (!(this.TargetActivity is BaseDialogFragment.IDialogClickListener))
					throw new InvalidCastException ("Calling activity " + this.TargetActivity.Class.SimpleName + " must implement IDialogClickListener interface");
			}
		}

		public async Task StartAsync (Task task) {
			await task.StartAsync (this);
		}

		public async Task<T> StartAsync<T> (Task<T> task) {
			return await task.StartAsync (this);
		}

		public async void Start (Task task) {
			await task.StartAsync (this);
		}

		public async void Start<T> (Task<T> task) {
			await task.StartAsync (this);
		}
	}
}

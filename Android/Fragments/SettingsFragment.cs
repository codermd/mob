using System;
using Android.Support.V4.App;
using Android.OS;
using Mxp.Droid.Adapters;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Android.Widget;
using Android.Content;
using Android.Views;

namespace Mxp.Droid
{
	public class SettingsFragment : ListFragment
	{
		private string[] items;

		public override void OnCreate (Bundle savedInstanceState) {
			base.OnCreate (savedInstanceState);

			this.HasOptionsMenu = true;

			this.items = new string[] {
				Labels.GetLoggedUserLabel (Labels.LabelEnum.IconsLegend),
				Labels.GetLoggedUserLabel (Labels.LabelEnum.Website),
				Labels.GetLoggedUserLabel (Labels.LabelEnum.Logout),
				"v." + this.Activity.PackageManager.GetPackageInfo (this.Activity.PackageName, 0).VersionName
			};
			this.ListAdapter = new ArrayAdapter<string> (this.Activity, Resource.Layout.List_settings_item, items);
		}

		public override void OnPrepareOptionsMenu (IMenu menu) {
			base.OnPrepareOptionsMenu (menu);

			menu.Clear ();
		}

		public override void OnListItemClick (ListView l, Android.Views.View v, int position, long id) {
			if (position > 2)
				return;
			
			Intent intent = null;

			switch (position) {
				case 0:
					intent = new Intent (this.Activity, typeof(SettingsIconsLegendActivity));
					break;
				case 1:
					intent = new Intent (Intent.ActionView, Android.Net.Uri.Parse (LoggedUser.Instance.BrowserLink));
					break;
				case 2:
					LoggedUser.Instance.Logout ();
					intent = new Intent (this.Activity, typeof (LoginActivity));
					intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
					break;
			}

			this.StartActivity (intent);
		}
	}
}
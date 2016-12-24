using System;
using Android.OS;
using Android.Widget;
using Mxp.Droid.Adapters;
using Android.Views;
using Mxp.Core.Business;

namespace Mxp.Droid
{		
	public class SettingsIconsLegendActivity : BaseActivity
	{
		protected override void OnCreate (Bundle bundle) {
			base.OnCreate (bundle);

			this.SetContentView (Resource.Layout.Settings_list_icons_legend);

			Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.Toolbar);
			this.SetSupportActionBar (toolbar);

			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			this.Title = Labels.GetLoggedUserLabel (Labels.LabelEnum.IconsLegend);
	
			ListView listView = this.FindViewById<ListView> (Resource.Id.List);

			listView.Adapter = new SettingsIconsLegendAdapter (this);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					this.Finish ();
					return true;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}
using System;

using Mxp.Droid.Helpers;
using Android.Widget;
using Android.Views;

namespace Mxp.Droid.ViewHolders
{
	public class SectionHeaderViewHolder : ViewHolder<string> {
		private TextView Text { get; set; }

		public SectionHeaderViewHolder (View convertView) {
			this.Text = convertView.FindViewById<TextView> (Resource.Id.Text);
		}

		public override void BindView (string title) {
			this.Text.Text = title;
		}
	}
}
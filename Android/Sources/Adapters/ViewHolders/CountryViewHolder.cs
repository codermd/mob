using Android.Widget;
using Android.Views;
using Mxp.Core.Business;

using Mxp.Droid.Helpers;
using Android.Graphics;

namespace Mxp.Droid.ViewHolders
{
	public class CountryViewHolder : ViewHolder<Country>
	{
		private TextView Text { get; set; }

		public CountryViewHolder (View convertView) {
			this.Text = convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
		}

		public override void BindView (Country country) {
			this.Text.Text = country.VName;

			this.Text.SetPadding (country.PaddingLeft * 50, this.Text.PaddingTop, this.Text.PaddingRight, this.Text.PaddingBottom);

			Text.SetTypeface (null, country.IsMatched ? TypefaceStyle.Bold : TypefaceStyle.Normal);
		}
	}
}
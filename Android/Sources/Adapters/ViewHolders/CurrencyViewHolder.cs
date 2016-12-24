using System;

using Android.Widget;
using Android.Views;

using Mxp.Droid.Helpers;
using Mxp.Core.Business;

namespace Mxp.Droid.ViewHolders
{
	public class CurrencyViewHolder : ViewHolder<Currency>
	{
		private TextView Text { get; set; }

		public CurrencyViewHolder (View convertView) {
			this.Text = convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
		}

		public override void BindView (Currency currency) {
			this.Text.Text = currency.VName;
		}
	}
}
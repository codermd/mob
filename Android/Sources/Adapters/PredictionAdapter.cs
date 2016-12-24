using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Mxp.Core.Services;
using Mxp.Core.Services.Google;
using Mxp.Droid.Adapters;
using Mxp.Droid.Helpers;
using Mxp.Droid.Sources.Adapters.Filters;
using Mxp.Core.Business;

namespace Mxp.Droid.Sources.Adapters
{
	public class PredictionAdapter : AbstractSectionAdapter<WrappedObject, BaseSectionAdapter<WrappedObject>>, IFilterable
	{
		public List<Prediction> Predictions { get; set; } = new List<Prediction> ();
		public Country Country { get; }

		private Filter mFilter;
		private View mGoogleConvertView;

		public PredictionAdapter (BaseSectionAdapter<WrappedObject> parentAdapter, Activity activity, string title, GoogleService.PlaceTypeEnum mRequestType, Country country = null) : base (parentAdapter, activity, title) {
			this.mFilter = new PredictionFilter (this, mRequestType);
			this.Country = country;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override int Count => this.Predictions.Count + 1;

		public override WrappedObject this [int position] => position == this.Predictions.Count ? new WrappedObject (null) : new WrappedObject (this.Predictions [position]);

		public override View GetView (int position, View convertView, ViewGroup parent) {
			if (position == this.Predictions.Count) {
				if (this.mGoogleConvertView == null)
					this.mGoogleConvertView = this.Activity.LayoutInflater.Inflate (Resource.Layout.Google_icon_item, parent, false);

				return this.mGoogleConvertView;
			}

			PredictionViewHolder viewHolder = null;

			if (convertView == null || convertView.Tag == null || !(convertView.Tag is PredictionViewHolder)) {
				convertView = this.Activity.LayoutInflater.Inflate (Android.Resource.Layout.SimpleDropDownItem1Line, parent, false);
				viewHolder = new PredictionViewHolder (convertView);
				convertView.Tag = viewHolder;
			} else
				viewHolder = convertView.Tag as PredictionViewHolder;

			viewHolder.BindView (this [position].GetInstance<Prediction> ());

			return convertView;
		}

		public Filter Filter => this.mFilter;

		public override bool IsEnabled (int position) {
			return position != this.Predictions.Count;
		}

		private class PredictionViewHolder : ViewHolder<Prediction>
		{
			private TextView TextView { get; set; }

			public PredictionViewHolder (View convertView) {
				this.TextView = convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
			}

			public override void BindView (Prediction prediction) {
				this.TextView.Text = prediction.description;
			}
		}
	}
}
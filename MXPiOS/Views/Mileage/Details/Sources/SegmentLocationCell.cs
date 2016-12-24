using System;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public class SegmentLocationCell : UITableViewCell
	{

		public SegmentLocationCell (string identifier) : base (UITableViewCellStyle.Default, identifier) {

		}
		private MileageSegment _mileageSegment;
		public MileageSegment MileageSegment {
			set {
				if (this._mileageSegment != null) {
					this._mileageSegment.PropertyChanged -= HandlePropertyChanged;
				}

				this._mileageSegment = value;

				if (this._mileageSegment != null) {
					this._mileageSegment.PropertyChanged += HandlePropertyChanged;
					this.refresh ();
				}
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			this._mileageSegment.PropertyChanged -= HandlePropertyChanged;
		}

		void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.refresh ();
		}

		private void refresh() {
			this.TextLabel.Text = this._mileageSegment.LocationAliasName;
		}

	}
}


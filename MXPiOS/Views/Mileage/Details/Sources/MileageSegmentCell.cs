
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class MileageSegmentCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("MileageSegmentCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("MileageSegmentCell");

		public MileageSegmentCell (IntPtr handle) : base (handle)
		{
		}

		public static MileageSegmentCell Create ()
		{
			return (MileageSegmentCell)Nib.Instantiate (null, null) [0];
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
			this.PositionLabel.Text = this._mileageSegment.LocationAliasName;
		}

	}
}


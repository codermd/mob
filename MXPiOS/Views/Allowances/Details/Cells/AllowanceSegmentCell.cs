using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	public partial class AllowanceSegmentCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("AllowanceSegmentCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("AllowanceSegmentCell");

		private static int WIDTH = 22;

		public AllowanceSegmentCell (IntPtr handle) : base (handle)
		{
		}

		public static AllowanceSegmentCell Create ()
		{
			return (AllowanceSegmentCell)Nib.Instantiate (null, null) [0];
		}

		private AllowanceSegment _allowanceSegment;
		public AllowanceSegment AllowanceSegment {
			set { 
				if (this._allowanceSegment != null) {
					this._allowanceSegment.PropertyChanged -= HandlePropertyChanged;
				}

				this._allowanceSegment = value;

				this._allowanceSegment.PropertyChanged += HandlePropertyChanged;
				this.refreshView ();
			}
			get {
				return this._allowanceSegment;
			}
		}

		void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.refreshView ();
		}
		public void refreshView(){
			this.AmoutLabel.Text = this.AllowanceSegment.VAmount.ToString();
			this.FromLabel.Text = this.AllowanceSegment.VDateFrom;
			this.FromLabel.TextColor = UIColor.FromRGB (0, 168, 198);
			this.ToLabel.Text = this.AllowanceSegment.VDateTo;
			this.ToLabel.TextColor = UIColor.FromRGB (0, 168, 198);


			this.AllowanceContainerBox.Layer.ShadowColor = UIColor.FromRGBA (0, 0, 0, 60).CGColor;
			this.AllowanceContainerBox.Layer.ShadowOffset = new CGSize (0, 1);
			this.AllowanceContainerBox.Layer.ShadowOpacity = 2.0f;
			this.AllowanceContainerBox.Layer.ShadowRadius = 1;
			this.AllowanceContainerBox.Layer.CornerRadius = 2;


			this.BreakfastWidth.Constant = this.AllowanceSegment.CanShowBreakfast ? WIDTH : 0;
			this.BreakfastImage.Highlighted = this.AllowanceSegment.Breakfast;

			this.LunchWidth.Constant = this.AllowanceSegment.CanShowLunch ? WIDTH : 0;
			this.LunchImage.Highlighted = this.AllowanceSegment.Lunch;

			this.DinnerWidth.Constant = this.AllowanceSegment.CanShowDinner ? WIDTH : 0;
			this.DinnerImage.Highlighted = this.AllowanceSegment.Dinner;

			this.BedWidth.Constant = this.AllowanceSegment.CanShowLodging ? WIDTH : 0;
			this.LodgingImage.Highlighted = this.AllowanceSegment.Lodging;

			this.InformationWidth.Constant = this.AllowanceSegment.CanShowInfo ? WIDTH : 0;
			this.InfoImage.Highlighted = this.AllowanceSegment.Info;

			this.MoonWidth.Constant = this.AllowanceSegment.CanShowWorkNight ? WIDTH : 0;
			this.WorkNightImage.Highlighted = this.AllowanceSegment.WorkNight;


			this.CountryImage.Image = UIImage.FromBundle (this.AllowanceSegment.Country.VFormattedResourceName);
			if (this.CountryImage.Image == null) {
				this.CountryImage.Image = UIImage.FromBundle ("NoFlag.png");
			}

		}
	}
}
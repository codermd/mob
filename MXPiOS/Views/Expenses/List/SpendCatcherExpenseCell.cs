using System;

using Foundation;
using UIKit;
using Mxp.Core;
using SDWebImage;
using CoreGraphics;
using Mxp.Core.Business;

namespace Mxp.iOS
{
	[Register ("SpendCatcherExpenseCell")]
	public partial class SpendCatcherExpenseCell : UITableViewCell
	{

		public static readonly UINib Nib = UINib.FromName ("SpendCatcherExpenseCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("SpendCatcherExpenseCell");

		private UIImageView ByCardIcon
		{
			get {
				return this.CountryImage.Superview.Subviews[1] as UIImageView;
			}
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			this.Container.Layer.ShadowColor = UIColor.FromRGBA (0, 0, 0, 60).CGColor;
			this.Container.Layer.ShadowOffset = new CGSize (0, 1);
			this.Container.Layer.ShadowOpacity = 2.0f;
			this.Container.Layer.ShadowRadius = 1;
			this.Container.Layer.CornerRadius = 2;

			this.CategoryLabel.TextColor = UIColor.FromRGB(0,168,198);

		}

		public static SpendCatcherExpenseCell Create ()
		{
			return (SpendCatcherExpenseCell)Nib.Instantiate (null, null) [0];
		}

		public SpendCatcherExpenseCell (IntPtr handle) : base (handle)
		{
		}

		public void Configure(SpendCatcherExpense e) {
			if (e.Product != null) {
				this.CategoryLabel.Text = e.Product.ExpenseCategory.Name;
			} else {
				this.CategoryLabel.Text = String.Empty;
			}
			if (e.Country != null) {
				this.CountryImage.Hidden = false;
				this.CountryImage.Image = UIImage.FromBundle (e.Country.VFormattedResourceName);
				if (this.CountryImage.Image == null) {
					this.CountryImage.Image = UIImage.FromBundle ("NoFlag.png");
				}
			} else {
				this.CountryImage.Image = UIImage.FromBundle ("NoFlag.png");
			}

			if (!string.IsNullOrEmpty (e.AttachmentPath)) {
				this.ReceiptImage.SetImage (NSUrl.FromString (e.AttachmentPath), UIImage.FromBundle("LoadingImage.png"), SDWebImageOptions.ContinueInBackground, (image, error, args1, args2)=>{
					this.ReceiptImage.ContentMode = UIViewContentMode.ScaleAspectFill;
				});
				this.ReceiptImage.ClipsToBounds = true;
			}
			this.DateLabel.Text = e.VDate;

			this.ByCardIcon.Hidden = !e.IsPaidByCC;
		}

		[Outlet]
		UIKit.UILabel DateLabel { get; set; }

		[Outlet]
		public UIKit.UILabel CategoryLabel { get; set; }

		[Outlet]
		UIKit.UIView Container { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CategoryLabel != null) {
				CategoryLabel.Dispose ();
				CategoryLabel = null;
			}
			if (ReceiptImage != null) {
				ReceiptImage.Dispose ();
				ReceiptImage = null;
			}

			if (CountryImage != null) {
				CountryImage.Dispose ();
				CountryImage = null;
			}

			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}

			if (Container != null) {
				Container.Dispose ();
				Container = null;
			}

		}

		[Outlet]
		public UIKit.UIImageView CountryImage { get; set; }

		[Outlet]
		public UIKit.UIImageView ReceiptImage { get; set; }



	}
}

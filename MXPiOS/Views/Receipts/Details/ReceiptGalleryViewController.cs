
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

using SDWebImage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mxp.iOS
{
	public partial class ReceiptGalleryViewController : MXPViewController
	{
		public ReceiptGalleryViewController () : base ("ReceiptGalleryViewController", null)
		{
		}

		public Receipts Receipts;
		public Receipt CurrentReceipt;

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.ScrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return this.ImageView;};
		
			if (this.CurrentReceipt == null) {
				this.showFirstReceipt ();
			} else {
				this.loadImage ();
			}

			this.CloseButton.SetTitle(Labels.GetLoggedUserLabel (Labels.LabelEnum.Close), UIControlState.Normal);
			this.DeleteButton.SetTitle(Labels.GetLoggedUserLabel (Labels.LabelEnum.Delete) + "?", UIControlState.Normal);
			this.NextButton.SetTitle(Labels.GetLoggedUserLabel (Labels.LabelEnum.Next), UIControlState.Normal);
			this.PreviousButton.SetTitle(Labels.GetLoggedUserLabel (Labels.LabelEnum.Previous), UIControlState.Normal);
			this.DeleteButton.Hidden = !this.Receipts.CanDelete;

		}
			
		public void showFirstReceipt() {
			if(this.Receipts.Count == 0) {
				return;
			}
			this.CurrentReceipt = this.Receipts [0];
			this.loadImage ();
		}

		partial void ClickOnNext (NSObject sender)
		{
			int index = this.Receipts.IndexOf(this.CurrentReceipt);
			int nextIndex = index+1; 
			if(nextIndex == this.Receipts.Count) {
				nextIndex = 0;
			}
			this.CurrentReceipt = this.Receipts[nextIndex];
			this.loadImage();
		}

		public void loadImage() {
			this.ReceiptPositionLabel.Text = (1+this.Receipts.IndexOf(this.CurrentReceipt)).ToString()+ "/"+this.Receipts.Count.ToString();
			if (this.CurrentReceipt.base64 != null) {
				NSData image = new NSData (this.CurrentReceipt.base64, NSDataBase64DecodingOptions.None);
				this.ImageView.Image = UIImage.LoadFromData (image);
				this.ScrollView.ContentSize = this.ImageView.Image.Size;
				this.ScrollView.ZoomToRect(new CGRect(new CGPoint(0, 0), this.ImageView.Image.Size), false);
			} else {

				if (this.CurrentReceipt.IsDocument) {
					this.ImageView.Image = UIImage.FromBundle ("DocumentExpenseCell.png");
				} else {
					this.ImageView.SetImage (NSUrl.FromString (this.CurrentReceipt.AttachmentPath), UIImage.FromBundle("LoadingImage.png"), SDWebImageOptions.ContinueInBackground, (image, options, args1, args2)=> {
						if(image == null) {
							return;
						}
						this.ScrollView.ContentSize = image.Size;
						this.ScrollView.ZoomToRect(new CGRect(new CGPoint(0, 0), image.Size), false);
					});
				}
			}
		}

		partial void ClickOnPrevious (NSObject sender)
		{
			int index = this.Receipts.IndexOf(this.CurrentReceipt);
			int previousIndex = index-1; 
			if(previousIndex < 0) {
				previousIndex = this.Receipts.Count-1;
			}
			this.CurrentReceipt = this.Receipts[previousIndex];
			this.loadImage();
		}

		partial void ClickOnClose (NSObject sender)
		{
			this.DismissViewController(true, null);
		}

		partial void ClickOnDelete (NSObject sender)
		{
			this.processDelete();
		}

		public async void processDelete() {
			int index = this.Receipts.IndexOf (this.CurrentReceipt);

			LoadingView.showMessage (Labels.GetLoggedUserLabel (Labels.LabelEnum.Deleting) + "...");

			try {
				await this.Receipts.DeleteReceipt(this.CurrentReceipt);
			} catch (Exception e) {
				MainNavigationController.Instance.showError(e);
				return;
			} finally {
				LoadingView.hideMessage ();
			}

			if (this.Receipts.Count == 0) {
				this.PresentingViewController.DismissViewController (true, null);
			} else {
				this.CurrentReceipt = this.Receipts [this.Receipts.Count == index ? index - 1 : index];
				this.loadImage ();
			}
		}
	}
}

using System;

using UIKit;
using Mxp.Core;
using SDWebImage;
using Foundation;
using Mxp.Core.Business;

namespace MXPiOS
{
	public partial class SpendCatcherViewController : UIViewController
	{

		SpendCatcherExpense Spendcatcher { get; set; }
		public SpendCatcherViewController (SpendCatcherExpense spendcatcher) : base ("SpendCatcherViewController", null)
		{
			this.Spendcatcher = spendcatcher;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			this.Title = Labels.GetLoggedUserLabel(Labels.LabelEnum.SpendCatcher);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (!string.IsNullOrEmpty (this.Spendcatcher.AttachmentPath)) {
				this.ImageView.SetImage (NSUrl.FromString (this.Spendcatcher.AttachmentPath), UIImage.FromBundle("LoadingImage.png"), SDWebImageOptions.ContinueInBackground, (image, error, args1, args2)=>{
					InvokeOnMainThread(()=>{
						this.ImageHeightConstraint.Constant = (int)image.Size.Height;
						this.ImageWidthConstraint.Constant = (int)image.Size.Width;
					});
					this.ImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
				});
			}
			this.ScrollView.Delegate = new ScrollDelegate(this.ImageView);
			this.ScrollView.MinimumZoomScale = (nfloat)0.5;
			this.ScrollView.MaximumZoomScale = (nfloat)1.5;
		}
			
		class ScrollDelegate : UIScrollViewDelegate {

			UIView ZoomView;
			public ScrollDelegate(UIView zoomView): base() {
				this.ZoomView = zoomView;
			}
			public override UIView ViewForZoomingInScrollView (UIScrollView scrollView)
			{
				return this.ZoomView;
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}



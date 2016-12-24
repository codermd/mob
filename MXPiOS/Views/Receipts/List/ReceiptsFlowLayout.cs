using System;
using UIKit;
using Mxp.Core.Business;
using Foundation;
using CoreGraphics;
using Mxp.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mxp.iOS
{
	public class SelectImageEventArgs : EventArgs
	{
		public string base64 { get; set;}
	}

	public class ReceiptsFlowLayout: UICollectionViewDelegateFlowLayout
	{

		public event EventHandler<SelectImageEventArgs> SelectImage = delegate {};

		public UIViewController viewController;
		public Receipts Receipts;
		public UICollectionView collectionView;

		public bool Editable { get; set;}

		public override CGSize GetSizeForItem (UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{

			if(this.Receipts.CanAdd && indexPath.Row == 0) {
				return new CGSize (collectionView.Bounds.Size.Width, 121);
			}
			return new CGSize (90, 150);
		}


		private Actionables getChoices()
		{

			List<Actionable> actions = new List<Actionable> ();

			if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.PhotoLibrary)) {
				actions.Add (
					new Actionable (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.ChooseFromLibrary), ()=>{
						this.chooseAPicture(null);
					})
				);
			}

			if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera)) {
				actions.Add (
					new Actionable (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.TakeAPicture), ()=>{
						this.takePicture(null);
					})
				);
			}

			return new Actionables ("Choose source", actions);
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (this.Receipts.CanAdd && indexPath.Row == 0) {
				new ActionnablesWrapper (this.getChoices (), this.viewController, collectionView.CellForItem (indexPath)).show ();
				return;
			}

			var index = this.Receipts.CanAdd ? indexPath.Row - 1 : indexPath.Row;
			var currentReceipt = this.Receipts [index];
			if (currentReceipt.IsDocument) {
				UIApplication.SharedApplication.OpenUrl(new NSUrl(currentReceipt.AttachmentPath));
				return;
			}

			ReceiptGalleryViewController gvc = new ReceiptGalleryViewController ();
			gvc.Receipts = this.Receipts;
			gvc.CurrentReceipt = currentReceipt;
			this.viewController.PresentViewController (gvc, true, null);

		}

		public void takePicture(object action){
			this.selectPicture (UIImagePickerControllerSourceType.Camera);
		}

		public void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e){

			UIImage image = e.Info [UIImagePickerController.OriginalImage] as UIImage;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {

				(sender as UIViewController).DismissViewController (true, () => {
					this.uploadingImage (image);		
					this.imagePicker = null;	
				});


			} else {
				this.imagePicker.DismissViewController(true, ()=>{
					this.uploadingImage (image);
				});
			}
		}

		public void uploadingImage(UIImage image) {
			string base64 = this.compressImage(image).GetBase64EncodedString(NSDataBase64EncodingOptions.None);
			this.SelectImage (null, new SelectImageEventArgs (){ base64 = base64 });	
		}


		public void Handle_Canceled(object sender, EventArgs e){

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.pickerPopover.Dismiss (true);
			} else {
				this.imagePicker.DismissModalViewController (true);
			}
		}

		public void chooseAPicture(object action){
			this.selectPicture (UIImagePickerControllerSourceType.PhotoLibrary);
		}


		public UIImagePickerController imagePicker;
		public UIPopoverController pickerPopover;

		public void selectPicture(UIImagePickerControllerSourceType source){

			this.imagePicker =  new  UIImagePickerController ();
			this.imagePicker.SourceType = source;

			this.imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
			this.imagePicker.Canceled += Handle_Canceled;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				if (!UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
					if (source == UIImagePickerControllerSourceType.Camera) {
						this.viewController.PresentModalViewController (this.imagePicker, true);
					} else {
						this.imagePicker.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
						this.viewController.PresentViewController (this.imagePicker, true, null);
					}
				} else {
					this.pickerPopover = new UIPopoverController (this.imagePicker);
					this.pickerPopover.PresentFromRect(
						this.collectionView.CellForItem(NSIndexPath.FromRowSection(0,0)).Frame, 
						this.collectionView, 
						UIPopoverArrowDirection.Any, 
						true
					);
				}
			} else {
				this.viewController.NavigationController.PresentModalViewController (this.imagePicker, true);
			}
		}

		public override bool ShouldShowMenu (UICollectionView collectionView, NSIndexPath indexPath)
		{
			return true;
		}

		public NSData compressImage(UIImage image) {
			float actualHeight = (float)image.Size.Height;
			float actualWidth = (float)image.Size.Width;
			float maxHeight = 1200.0f; //new max. height for image
			float maxWidth = 1200.0f; //new max. width for image
			float imgRatio = actualWidth/actualHeight;
			float maxRatio = maxWidth/maxHeight;
			float compressionQuality = 0.6f; //80 percent compression

			if (actualHeight > maxHeight || actualWidth > maxWidth){ 
				if(imgRatio < maxRatio){
					//adjust width according to maxHeight
					imgRatio = maxHeight / actualHeight;
					actualWidth = imgRatio * actualWidth;
					actualHeight = maxHeight;
				}
				else if(imgRatio > maxRatio){
					//adjust height according to maxWidth
					imgRatio = maxWidth / actualWidth;
					actualHeight = imgRatio * actualHeight;
					actualWidth = maxWidth;
				}
				else{
					actualHeight = maxHeight;
					actualWidth = maxWidth;
				}
			}

			CGRect rect = new CGRect (0, 0, actualWidth, actualHeight);
			UIGraphics.BeginImageContext (rect.Size);
			image.Draw (rect);
			UIImage img = UIGraphics.GetImageFromCurrentImageContext ();
			NSData data = img.AsJPEG (compressionQuality);
			UIGraphics.EndImageContext ();

			return data;
		}
	}
}
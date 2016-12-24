
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using Mxp.Core.Business;

using SDWebImage;

namespace Mxp.iOS
{
	public partial class ReceiptCell : UICollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("ReceiptCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ReceiptCell");

		private Receipt Receipt;

		public ReceiptCell (IntPtr handle) : base (handle)
		{
		}

		public static ReceiptCell Create ()
		{
			return (ReceiptCell)Nib.Instantiate (null, null) [0];
		}

		public void setReceipt(Receipt receipt){

			if (receipt.IsDocument) {
				this.ReceiptImage.Image = UIImage.FromBundle ("DocumentExpenseCell.png");
				return;
			}

			this.ReceiptImage.ContentMode = UIViewContentMode.Center;
			this.Receipt = receipt;
			if (receipt.base64 != null) {
				NSData image = new NSData(receipt.base64, NSDataBase64DecodingOptions.None);
				this.ReceiptImage.Image =  UIImage.LoadFromData(image);
				this.ReceiptImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			} else {
				this.ReceiptImage.SetImage (NSUrl.FromString (this.Receipt.AttachmentPath), UIImage.FromBundle("LoadingImage.png"), SDWebImageOptions.ContinueInBackground, (image, error, args1, args2)=>{
					this.ReceiptImage.ContentMode = UIViewContentMode.ScaleAspectFill;
				});
			}
		}
	}
}
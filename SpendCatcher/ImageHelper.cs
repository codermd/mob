using System;
using Foundation;
using UIKit;
using CoreGraphics;


namespace sc
{
	public class ImageHelper
	{

		public static NSData compressImage(UIImage image) {
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


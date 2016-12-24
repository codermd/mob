using System.ComponentModel;
using Foundation;
using UIKit;
using Mxp.Core;

namespace sc
{

	public class SpendCatcherItem : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void NotifyPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public SpendCatcherItem(UIImage image)
		{
			this._selectedImage = image;
		}

		private Country _selectedCountry = null;
		public Country SelectedCountry
		{
			get
			{
				return this._selectedCountry;
			}
			set
			{
				this._selectedCountry = value;
				this.NotifyPropertyChanged("change-SelectedCountry");
			}
		}

		public bool TransactionByCard = false;

		private Product _selectedProduct = null;
		public Product SelectedProduct
		{
			get
			{
				return this._selectedProduct;
			}
			set
			{
				this._selectedProduct = value;
				this.NotifyPropertyChanged("change-SelectedProduct");
			}
		}

		public override string ToString()
		{
			return string.Format("[SpendCatcherItem: SelectedCountry={0}, SelectedProduct={1}, SelectedImage={2}]", SelectedCountry, SelectedProduct, SelectedImage);
		}

		private UIImage _selectedImage = null;
		public UIImage SelectedImage
		{
			get
			{
				return this._selectedImage;
			}
			set
			{
				this._selectedImage = value;
				this.NotifyPropertyChanged("change-SelectedImage");
			}
		}


		public NSUrlRequest GenerateRequest()
		{

			var base64 = ImageHelper.compressImage(this.SelectedImage).GetBase64EncodedString(NSDataBase64EncodingOptions.None);

			NSDictionary header = NSDictionary.FromObjectAndKey(NSObject.FromObject("application/x-www-form-urlencoded"), NSObject.FromObject("content-type"));
			NSMutableData postData = new NSMutableData();
			postData.AppendData(NSData.FromString("MXPSessionSharedKey=" + Context.Instance.token, NSStringEncoding.UTF8));
			postData.AppendData(NSData.FromString("&appVersion=" + Context.Instance.sharedUserDefault.StringForKey(AppExtensionSharedKeys.VERSION_KEY), NSStringEncoding.UTF8));
			postData.AppendData(NSData.FromString("&typeOs=I", NSStringEncoding.UTF8));
			postData.AppendData(NSData.FromString("&ImageData=" + base64, NSStringEncoding.UTF8));
			postData.AppendData(NSData.FromString("&ObjectType=tempTrx", NSStringEncoding.UTF8));
			postData.AppendData(NSData.FromString("&fileType=image/jpeg", NSStringEncoding.UTF8));
			postData.AppendData(NSData.FromString("&ImageName=mobileUpload.jpg", NSStringEncoding.UTF8));

			postData.AppendData(NSData.FromString("&IsPaidByCC=" + this.TransactionByCard, NSStringEncoding.UTF8));


			if (this.SelectedCountry != null)
				postData.AppendData(NSData.FromString("&CountryID=" + this.SelectedCountry.Id.ToString(), NSStringEncoding.UTF8));

			if (this.SelectedProduct != null)
				postData.AppendData(NSData.FromString("&ProductID=" + this.SelectedProduct.Id.ToString(), NSStringEncoding.UTF8));

			string api = "";

			if (Context.USE_FAKE_DATA)
			{
				api = FakeData.api;
			}
			else {
				api = Context.Instance.sharedUserDefault.StringForKey(AppExtensionSharedKeys.POST_URL_KEY);
			}

			NSMutableUrlRequest request = new NSMutableUrlRequest(NSUrl.FromString(api), NSUrlRequestCachePolicy.UseProtocolCachePolicy, 30.0);
			request.HttpMethod = "POST";
			request.Headers = header;
			request.Body = postData;

			return request;
		}



	}
}


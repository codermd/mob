using System;
using sc;
using UIKit;

namespace SpendCatcher
{
	public partial class SpendCatcherViewController : UIViewController
	{
		public SpendCatcherViewController() : base("SpendCatcherViewController", null)
		{
		}

		private SpendCatcherItem _spendCatcher;
		public SpendCatcherItem SpendCatcher { 
			get {
				return this._spendCatcher;
			}
			set {
				_spendCatcher = value;
			}
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.configureView();

			Context.Instance.AppendLogs("\nRegister for event \n");
			this.SpendCatcher.PropertyChanged += SpendCatcherPropertyChange;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			this.SpendCatcher.PropertyChanged -= SpendCatcherPropertyChange;
		}

		public void SpendCatcherPropertyChange(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName.Equals("change-SelectedCountry")) {
				InvokeOnMainThread(() => {
					this.configureView();
				});
			}
			if (e.PropertyName.Equals("change-SelectedProduct")) {
				InvokeOnMainThread(() =>
				{
					this.configureView();
				});
			}
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			Context.Instance.AppendLogs("Width " + this.View.Frame.Width);
		}

		void configureView() {
			Context.Instance.AppendLogs("\nConfigure View..." + this.SpendCatcher.ToString());

			try {
				this.ProductButton.SetTitle(Context.Instance.Labels[42], UIControlState.Normal);
				this.CountryButton.SetTitle(Context.Instance.Labels[39], UIControlState.Normal);
				this.TransactionLabel.Text = Context.Instance.Labels[3678];
			}
			catch (Exception) {
				this.ProductButton.SetTitle("Category", UIControlState.Normal);
				this.CountryButton.SetTitle("Country", UIControlState.Normal);
				this.TransactionLabel.Text = "Credit Card";
			}

			if (this.SpendCatcher == null)
			{
				
				try
				{
					this.ProductLabel.Text = Context.Instance.Labels[377];
					this.CountryLabel.Text = Context.Instance.Labels[377];
				}
				catch (Exception) {
					this.ProductLabel.Text = "No selection";
					this.CountryLabel.Text = "No selection";
				}
				this.ImageView.Image = null;
				return;
			}

			if (this.SpendCatcher.SelectedProduct != null)
			{
				this.ProductLabel.Text = this.SpendCatcher.SelectedProduct.Name;
			}
			else {
				try
				{
					this.ProductLabel.Text = Context.Instance.Labels[377];
				}
				catch (Exception)
				{
					this.ProductLabel.Text = "No selection";
				}
			}

			if (this.SpendCatcher.SelectedCountry != null)
			{
				this.CountryLabel.Text = this.SpendCatcher.SelectedCountry.Name;
			}
			else {
				try
				{
					this.CountryLabel.Text = Context.Instance.Labels[377];
				}
				catch (Exception)
				{
					this.CountryLabel.Text = "No selection";
				}
			}

			this.TransactionSwitch.On = this.SpendCatcher.TransactionByCard;

			if (this.SpendCatcher.SelectedImage != null)
			{
				this.ImageView.Image = this.SpendCatcher.SelectedImage;
			}
			else {
				this.ImageView.Image = null;
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void ClickOnCountry(Foundation.NSObject sender)
		{
			this.MainViewController.showCountries(SpendCatcher);
		}

		partial void ClickOnProduct(Foundation.NSObject sender)
		{
			this.MainViewController.showProducts(SpendCatcher);
		}

		partial void SwitchTransaction(Foundation.NSObject sender)
		{
			this.SpendCatcher.TransactionByCard = this.TransactionSwitch.On;
		}

		MainViewController MainViewController {
			get {
				return (MainViewController) this.ParentViewController;
			}
		}
	}
}



using System;
using System.Collections.ObjectModel;
using CoreGraphics;
using Foundation;
using MobileCoreServices;
using ObjCRuntime;
using SpendCatcher;
using UIKit;

namespace sc
{
	public partial class MainViewController : UIViewController
	{
		public MainViewController() : base("MainViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.ContentView.Layer.CornerRadius = 5;
			this.ContentView.Layer.MasksToBounds = true;
			this.LogTextArea.Hidden = !Context.SHOW_DEBUG;
		}
		void launchImagesLoading() {
			NSExtensionItem items = this.ExtensionContext.InputItems[0];

			string title = "Upload";
			try {
				title = Context.Instance.Labels [2933];
			} catch (Exception) { }

			this.UploadButton.SetTitle (title + "(" + items.Attachments.Length + ")", UIControlState.Normal);

			this.Scrollview.UserInteractionEnabled = true;

			for (var i = 0; i < items.Attachments.Length; i++)
			{
				NSItemProvider itemProvider = items.Attachments[i];
				if (itemProvider.HasItemConformingTo(UTType.Image))
				{
					itemProvider.LoadItem(UTType.Image, null, (obj, loadError) =>
					{
						if (loadError != null)
						{
							return;
						}
						NSData data = NSData.FromUrl(obj as NSUrl);
						UIImage image = UIImage.LoadFromData(data);
						try
						{
							InvokeOnMainThread(() =>
							{
								var svc = new SpendCatcherViewController();
								this.AddChildViewController(svc);
								var newSpendCatcher = new SpendCatcherItem(image);
								Context.Instance.SpendCatchers.Add(newSpendCatcher);
								svc.SpendCatcher = newSpendCatcher;

								svc.WillMoveToParentViewController(this);
								this.AddImageInScroll(svc.View, i == items.Attachments.Length - 1, i);
								svc.DidMoveToParentViewController(this);

								this.configureLoadingView();

							});
						}
						catch (Exception e)
						{
							Context.Instance.AppendLogs(e.ToString());
						}
					});
				}
			}

			if (items.Attachments.Length == 1)
			{
				this.PageControl.Hidden = true;
			}
			else {
				this.PageControl.Hidden = false;
			}
			this.PageControl.Pages = items.Attachments.Length;
		}

		void configureLoadingView()
		{
			this.Loading.Hidden = this.ExtensionContext.InputItems[0].Attachments.Length == Context.Instance.SpendCatchers.Count;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Context.Instance.PropertyChanged += Context_Instance_PropertyChanged;
			Context.Instance.AppendLogs("Start");
			this.Scrollview.Scrolled += ScrolledEvent;

			if (this.CheckPermission())
			{
				this.launchImagesLoading();
				this.configureView();
			}


		}

		private void configureView(){
			this.SuccessView.Alpha = 0;
			try
			{
				this.SuccessAddLabel.Text = Context.Instance.Labels[4615];
				this.SuccessShowButton.SetTitle(Context.Instance.Labels[4616], UIControlState.Normal);
				this.SuccessCloseButton.SetTitle(Context.Instance.Labels[4607], UIControlState.Normal);

				this.UploadButton.SetTitle(Context.Instance.Labels[2933], UIControlState.Normal);
				this.CancelButton.SetTitle(Context.Instance.Labels[386], UIControlState.Normal);

			}
			catch (Exception)
			{
				this.SuccessAddLabel.Text = "ERROR";
			}
		}

		void showSuccessView() {
			this.View.LayoutIfNeeded ();

			this.SpendCatcherSelectorView.Alpha = 1;
			this.SuccessView.Alpha = 0;
			UIView.Animate (0.25, () => {
				this.SpendCatcherSelectorView.Alpha = 0;
				this.SuccessView.Alpha = 1;
				this.View.LayoutIfNeeded ();
			}, ()=> {
				this.SpendCatcherSelectorView.Hidden = true;
			});
		}

		private void ScrolledEvent(object sender, EventArgs e)
		{
			this.PageControl.CurrentPage = (System.nint)Math.Round(this.Scrollview.ContentOffset.X / (this.Scrollview.Frame.Width + 0.0));
			console("Scroll x " +this.Scrollview.ContentOffset.X);
			console("Scroll Width " + this.Scrollview.Frame.Width);
			console("Current page " + this.PageControl.CurrentPage);

		}


		void console(String message)
		{
			Context.Instance.AppendLogs("\n"+message+"\n");
		}

		Collection<UIView> previews = new Collection<UIView>();

		void AddImageInScroll(UIView iv, bool last, int index)
		{

			try
			{
				UIView previousImage = this.previews.Count > 0 ? this.previews[this.previews.Count - 1] : null;

				iv.ContentMode = UIViewContentMode.ScaleAspectFill;

				this.previews.Add(iv);

				CGRect frame = new CGRect();

				frame.Height = Scrollview.Frame.Height;
				frame.Width = Scrollview.Frame.Width;

				if (previousImage != null)
				{
					//Configure left part

					frame.X = previousImage.Frame.Width + previousImage.Frame.X;
					//this.contentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[previousImage]-[iv(" + this.Scrollview.Frame.Width + ")]", 0, null, dict));
				}
				else {
					frame.X = 0;
					//this.contentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-[iv(" + this.Scrollview.Frame.Width + ")]", 0, null, dict));
				}

				//if (last)
				//{
					//this.contentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:[iv(\" + this.Scrollview.Frame.Width + \")]-|", 0, null, dict));
					//this.Scrollview.ContentSize = new CGSize(previews.Count * Scrollview.Frame.Width, Scrollview.Frame.Height);
					//this.contentView.Frame = new CGRect(this.contentView.Frame.X, this.contentView.Frame.Y, previews.Count * Scrollview.Frame.Width, this.contentView.Frame.Height);
					//Context.Instance.AppendLogs("\n\n\n\n\n\n\n\n\n\n Content view" + frame.ToString());
				//}

				iv.Frame = frame;
				this.Scrollview.AddSubview(iv);
				Context.Instance.AppendLogs("\n item frame:" + frame.ToString() + "\n\n");


				//this.contentView.Frame = new CGRect(this.contentView.Frame.X, this.contentView.Frame.Y, previews.Count * Scrollview.Frame.Width, this.contentView.Frame.Height);
				this.Scrollview.ContentSize = new CGSize(previews.Count * Scrollview.Frame.Width, this.Scrollview.Frame.Height);
				Context.Instance.AppendLogs("\n ->size :" + this.Scrollview.ContentSize.ToString());

			}
			catch (Exception e) {
				Context.Instance.AppendLogs(e.ToString());
			}

		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			Context.Instance.PropertyChanged -= Context_Instance_PropertyChanged;
		}


		void Context_Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Log"))
			{
				InvokeOnMainThread(() =>
				{
					this.LogTextArea.Text = Context.Instance.Log;
					this.LogTextArea.SetContentOffset(new CGPoint(0, this.LogTextArea.ContentSize.Height - this.LogTextArea.Frame.Size.Height), true);
				});
			}
		}


		bool CheckPermission()
		{

			if (!Context.Instance.isAuthenticated)
			{
				var alert = UIAlertController.Create("No MobileXpense Account", "You are not logged in MobileXpense app. Please login and try again.", UIAlertControllerStyle.Alert);

				alert.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel, (evt) =>
				{
					this.dismiss(() =>
					{
						this.ExtensionContext.CompleteRequest(null, null);
					});
				}));

				alert.AddAction(UIAlertAction.Create("Login", UIAlertActionStyle.Default, (evt) =>
				{
					this.dismiss(() =>
					{
						UIResponder responder = this;
						while ((responder = responder.NextResponder) != null)
						{
							if (responder.RespondsToSelector(new Selector("openURL:")))
								responder.PerformSelector(new Selector("openURL:"), new NSUrl("mobilexpense://"));
						}
					});
				}));

				this.PresentViewControllerAsync(alert, true);
				return false;
			}

			if (!Context.Instance.isAccessible)
			{

				var alert = UIAlertController.Create(Context.Instance.Labels[4614], null, UIAlertControllerStyle.Alert);

				alert.AddAction(UIAlertAction.Create(Context.Instance.Labels[4607], UIAlertActionStyle.Cancel, (evt) =>
				{
					this.dismiss(() =>
					{
						this.ExtensionContext.CompleteRequest(null, null);
					});
				}));
				this.PresentViewControllerAsync(alert, true);
				return false;
			}

			return true;
		}


		partial void ClickOnCancel(Foundation.NSObject sender)
		{
			this.dismiss(() =>
			{
				this.ExtensionContext.CompleteRequest(null, (finished) =>
				{
					Context.Instance.Reset();
				});
			});
		}

		partial void ClickOnUpload(Foundation.NSObject sender)
		{

			Context.Instance.PostData();
			this.showSuccessView();
		}

		partial void ClickOnShow(NSObject sender)
		{
			this.ClickOnShowApp();
		}

		void dismiss(Action action)
		{

			this.View.LayoutIfNeeded();
			UIView.Animate(0.25, () =>
			{
				this.TransparentView.Alpha = 0;
				this.View.LayoutIfNeeded();
			}, () =>
			{
				if (action != null) {
					action();	
				}
			});

		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public void showProducts(SpendCatcherItem SpendCatcher) {
			var vc = new ProductsTableViewController(UITableViewStyle.Plain);
			vc.PreselectedProduct = SpendCatcher.SelectedProduct;
			var nvc = new UINavigationController(vc);

			vc.cellSelected += (sender, e) => {
				SpendCatcher.SelectedProduct = e.Product;
				nvc.DismissViewController(true, null);
			};

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			}
			else {
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			}

			this.PresentViewController(nvc, true, null);
		}

		public void showCountries(SpendCatcherItem SpendCatcher) {
			var vc = new CountriesTableViewController(UITableViewStyle.Plain);
			vc.PreselectedCountry = SpendCatcher.SelectedCountry;
			Context.Instance.AppendLogs("\n\n---->" + UIDevice.CurrentDevice.UserInterfaceIdiom);
			var nvc = new UINavigationController(vc);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
			}
			else {
				nvc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			}

			vc.cellSelected += (sender, e) =>
			{
				SpendCatcher.SelectedCountry = e.Country;
				nvc.DismissViewController(true, null);
			};
			this.PresentViewController(nvc, true, null);
		}


		void ClickOnShowApp ()
		{
			this.dismiss (()=>{
				UIResponder responder = this;
				while ((responder = responder.NextResponder) != null) {
					if (responder.RespondsToSelector (new Selector ("openURL:")))
						responder.PerformSelector (new Selector ("openURL:"), new NSUrl ("mobilexpense://"));
				}
				this.ExtensionContext.CompleteRequest(null, null);
			});
		}


	}
}

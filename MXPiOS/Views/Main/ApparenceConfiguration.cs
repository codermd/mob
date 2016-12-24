using System;
using UIKit;

namespace Mxp.iOS
{
	public class ApparenceConfiguration
	{
		public ApparenceConfiguration ()
		{

			UIColor defaultColor = UIColor.FromRGB(44,58,78);
			UITableViewCell.Appearance.TintColor = defaultColor;
			UILabel.Appearance.TextColor = defaultColor;
			UICollectionView.Appearance.TintColor = defaultColor;

			UINavigationBar.Appearance.TintColor = defaultColor;

			UITabBar.Appearance.TintColor = UIColor.FromRGB(0,168,198);
			UITabBarItem.Appearance.SetTitleTextAttributes ( new UITextAttributes () {
					Font = UIFont.FromName ("Avenir", 10)
			}, UIControlState.Normal);
					
			UINavigationBar.Appearance.SetTitleTextAttributes (new UITextAttributes () {
				Font = UIFont.FromName ("Avenir", 18),
				TextColor = UIColor.FromRGB(52,63,77)
			});

			UISegmentedControl.Appearance.SetTitleTextAttributes (new UITextAttributes() {
				Font = UIFont.FromName ("Avenir", 15)
			}, UIControlState.Normal);

			UIBarButtonItem.Appearance.SetTitleTextAttributes (new UITextAttributes() {
				Font = UIFont.FromName ("Avenir", 15),
			}, UIControlState.Normal);
				
			UILabel.AppearanceWhenContainedIn (typeof(UITableViewHeaderFooterView)).Font = UIFont.FromName ("Avenir", 14);
			UIView.AppearanceWhenContainedIn (typeof(UITableViewHeaderFooterView)).BackgroundColor = UIColor.FromRGB (230, 230, 230);
			UILabel.AppearanceWhenContainedIn (typeof(UITableViewHeaderFooterView)).TextColor = UIColor.FromRGB(52, 63, 77);

			UISegmentedControl.Appearance.TintColor = UIColor.FromRGB(0,168,198);

			UILabel.AppearanceWhenContainedIn (typeof(UITextField)).Font = UIFont.FromName ("Avenir", 14);

			UINavigationBar.Appearance.BarTintColor = UIColor.White;

			UITableViewCell.Appearance.TintColor = defaultColor;
			UILabel.Appearance.TextColor = defaultColor;

			UITableView.Appearance.SeparatorColor = UIColor.FromRGBA (0, 0, 0, 35);

		}
	}
}


using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            this.InitializeComponent();
            this.IconsLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.IconsLegend);
            this.DesktopLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Website);
            this.LogoutLabel.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.Logout);
            this.VersionLabel.Text = "Version";
            var thisPackage = Windows.ApplicationModel.Package.Current;
            var version = thisPackage.Id.Version;
            var appVersion = string.Format("{0}.{1}.{2}",
                version.Major, version.Minor, version.Build);  
            this.VersionLabel.Text = "v." + appVersion;
        }

        private void Icons_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(IconsPage));
        }
        private async void Desktop_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string uriToLaunch = @LoggedUser.Instance.BrowserLink;
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
        private void Logout_tapped(object sender, TappedRoutedEventArgs e)
        {
            MainController.Instance.MainPageClear();

            this.ProgressRing.IsActive = true;
            try
            {
                LoggedUser.Instance.Logout();
                this.ProgressRing.IsActive = false;
                ((Window.Current.Content as Frame).Content as Page).NavigationCacheMode = NavigationCacheMode.Disabled;
                ((Window.Current.Content as Frame).Content as Page).NavigationCacheMode = NavigationCacheMode.Required;
            }
            catch (ValidationError er)
            {
                MessageDialog messageDialog = new MessageDialog(er.Verbose);
                messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
                messageDialog.ShowAsync();
                this.ProgressRing.IsActive = false;
                return;
            }
            
            ((Frame)Window.Current.Content).Navigate(typeof(LoginPage));
            this.ProgressRing.IsActive = false;
        }
    }
}

using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class LoginPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public static bool IsLogged;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        private void InitializeNavigationHelper()
        {
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
        ShareOperation status;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is ShareOperation)
                status = e.Parameter as ShareOperation;
            while (navigationHelper.CanGoBack())
                Frame.BackStack.RemoveAt(0);
            while (navigationHelper.CanGoForward())
                Frame.ForwardStack.RemoveAt(0);
            Instance_CollapseWebViewRequest(null, null);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
        public LoginPage()
        {
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = Windows.System.UserProfile.GlobalizationPreferences.Languages[0];
            StatusBar statusBar = StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = new Windows.UI.Color() { A = 0xFF, R = 0xFF, G = 0xFF, B = 0xFF };
            RememberChecked = LoggedUser.Instance.AutoLogin;
            CompanyRememberMeChecked = LoggedUser.Instance.AutoLogin;
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            MainController.Instance.CollapseWebViewRequest += Instance_CollapseWebViewRequest;
            Instance_CollapseWebViewRequest(null, null);

            this.WebViewSAML.ContentLoading += WebViewSAML_ContentLoading;
            WebViewSAML.ScriptNotify += WebViewSAML_ScriptNotify;
            WebViewSAML.NavigationStarting += WebViewSAML_NavigationStarting;
        }
       

        private void WebViewSAML_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            String str = WebViewSAML.Source.ToString();
        }

        private async void WebViewSAML_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            try
            {
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, WebViewSAML.Source);

                var result = await client.SendAsync(request);
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();
                    if (content.Contains("mobilexpense"))
                    {
                        WebViewSAML.Stop();
                        this.WebViewSAML.Source = new Uri("about:blank");
                        this.WebViewSAML.Stop();
                        this.WebViewSAML.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void WebViewSAML_ScriptNotify(object sender, NotifyEventArgs e)
        {
            String str = WebViewSAML.Source.ToString();
        }

        private async void WebViewSAML_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            if (WebViewSAML.Source.Scheme.Equals("mobilexpense"))
                this.StopWebView();
        }

        private void StopWebView()
        {
            this.WebViewSAML.Source = new Uri("about:blank");
            this.WebViewSAML.Stop();
            this.WebViewSAML.Visibility = Visibility.Collapsed;
        }

        private void Instance_CollapseWebViewRequest(object sender, EventArgs e)
        {
            this.WebViewSAML.Source = new Uri("about:blank");
            this.WebViewSAML.Stop();
            this.WebViewSAML.Visibility = Visibility.Collapsed;
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }
        private async void TryLogin()
        {

            this.UserProgressRing.IsActive = true;
            this.CompanyProgressRing.IsActive = true;
            LoggedUser.Instance.Username = UserNameTextBox.Text;
            LoggedUser.Instance.Password = UserPasswordBox.Password;

            LoggedUser.Instance.Email = null;
            try
            {
                await LoggedUser.Instance.LoginAsync();
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.UserProgressRing.IsActive = false;
                this.CompanyProgressRing.IsActive = false;
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                this.UserProgressRing.IsActive = false;
                this.CompanyProgressRing.IsActive = false;
                return;
            }

            try
            {
                await LoggedUser.Instance.RefreshCacheAsync();
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.UserProgressRing.IsActive = false;
                this.CompanyProgressRing.IsActive = false;
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
            this.UserProgressRing.IsActive = false;
            this.CompanyProgressRing.IsActive = false;
            LoggedUser.Instance.TrackContext.AddViews("");
            if (status == null)
                Frame.Navigate(typeof(MainPage));


            else if (status != null)
            {
                if (!Preferences.Instance.IsSpendCatcherEnable)
                {
                    MessageDialog messageDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcherDisabledMessage));
                    messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Close)), (command) => { Application.Current.Exit(); }));
                    await messageDialog.ShowAsync();
                    return;
                }
                else
                {
                    Frame.Navigate(typeof(MainPage));
                    Frame.Navigate(typeof(SpendCatcherPage), status);
                }
            }
            status = null;
        }

        bool RememberChecked;
        bool CompanyRememberMeChecked;
        private TextBox UserNameTextBox;
        private PasswordBox UserPasswordBox;
        private CheckBox UserCheckBox;
        private Button UserLoginButton;
        private HyperlinkButton UserResetPassword;
        private ProgressRing UserProgressRing;
        private ProgressRing CompanyProgressRing;
        private TextBox CompanyEmailAdress;
        private CheckBox CompanyRememberMe;
        private Button CompanyLogin;

        private void Username_Loaded(object sender, RoutedEventArgs e)
        {
            UserNameTextBox = (TextBox)sender;
            UserNameTextBox.PlaceholderText = "Username";
            if (LoggedUser.Instance.VUsername != null)
                UserNameTextBox.Text = LoggedUser.Instance.VUsername;
        }

        private void Password_Loaded(object sender, RoutedEventArgs e)
        {
            UserPasswordBox = (PasswordBox)sender;
            UserPasswordBox.PlaceholderText = "Password";
            if (LoggedUser.Instance.Password != null)
                UserPasswordBox.Password = LoggedUser.Instance.Password;
        }
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            UserCheckBox = (CheckBox)sender;
            UserCheckBox.Content = "Remember Me";
            UserCheckBox.IsChecked = RememberChecked;
        }

        private void UserLoginButtonLoaded(object sender, RoutedEventArgs e)
        {
            UserLoginButton = (Button)sender;
            UserLoginButton.Content = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Login);
        }

        private void UserResetPasswordLoaded(object sender, RoutedEventArgs e)
        {
            UserResetPassword = (HyperlinkButton)sender;
        }

        private void UserProgressRingLoaded(object sender, RoutedEventArgs e)
        {
            UserProgressRing = (ProgressRing)sender;
        }

        private void CompanyEmailAdressLoaded(object sender, RoutedEventArgs e)
        {
            CompanyEmailAdress = (TextBox)sender;
            CompanyEmailAdress.PlaceholderText = "Company email adress";
        }

        private void CompanyLoginLoaded(object sender, RoutedEventArgs e)
        {
            CompanyLogin = (Button)sender;
        }

        private void CompanyRememberMeLoaded(object sender, RoutedEventArgs e)
        {
            CompanyRememberMe = (CheckBox)sender;
            CompanyRememberMe.IsChecked = CompanyRememberMeChecked;
        }
        private void CompanyProgressRingLoaded(object sender, RoutedEventArgs e)
        {
            CompanyProgressRing = (ProgressRing)sender;
        }

        private void RememberMe_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser.Instance.AutoLogin = (bool)((CheckBox)sender).IsChecked;
        }

        private async void MailLogin_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser.Instance.Email = CompanyEmailAdress.Text;
            LoggedUser.Instance.Username = null;
            LoggedUser.Instance.Password = null;
            try
            {
                string uriToLaunch = await LoggedUser.Instance.LoginByMailAsync();
                var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(uriToLaunch));
            }
            catch (Exception error)
            {
                MessageDialog messageDialog = new MessageDialog(error.GetExceptionMessage());
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                this.UserProgressRing.IsActive = false;
                this.CompanyProgressRing.IsActive = false;
            }
        }

        private void CompanyRememberMe_Clicked(object sender, RoutedEventArgs e)
        {
            LoggedUser.Instance.AutoLogin = (bool)((CheckBox)sender).IsChecked;
        }

        private void Username_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                UserPasswordBox.Focus(FocusState.Keyboard);
        }

        private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                TryLogin();
        }

        private async void UserResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = @LoggedUser.Instance.ForgotPasswordUrl;
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}


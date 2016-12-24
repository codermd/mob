using Caliburn.Micro;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Win.Helpers;
using Mxp.Win.Helpers.SchemeActions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace Mxp.Win
{
    public sealed partial class App : Application, StatesMediator.IStateListener
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
        private StatesMediator mStatesMediator;
#endif
        public App()
        {
            this.InitializeComponent();
            this.mStatesMediator = new StatesMediator(new CommandsFactory(), this);
            Application.Current.Suspending += this.OnSuspending;
            Application.Current.Resuming += OnResuming;
        }
        private void OnResuming(object sender, object o)
        {
            Debug.WriteLine("App.OnResuming");
        }
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();  // Max approx 5-secs to save state
            deferral.Complete();
        }
        bool ShareMode;
        protected async override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            Xamarin.Forms.Forms.Init(args);
            ShareMode = true;
            if(rootFrame == null)
                rootFrame = CreateRootFrame();
                
            if (!LoggedUser.Instance.IsAuthenticated)
                await AutoAuthentication();
            if (LoggedUser.Instance.IsAuthenticated)
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
                    rootFrame.Navigate(typeof(MainPage), args.ShareOperation);
                    rootFrame.Navigate(typeof(SpendCatcherPage), args.ShareOperation);

                }
            }
            else
                rootFrame.Navigate(typeof(LoginPage), args.ShareOperation);
            Window.Current.Activate();
        }

        private async Task AutoAuthentication()
        {
            if (LoggedUser.Instance.CanLogin && LoggedUser.Instance.AutoLogin)
            {
                await TryLoginAsync();
            }
            else if (LoggedUser.Instance.CanLoginByMail && LoggedUser.Instance.AutoLogin)
            {
                MailLoginAsync();
            }
        }

      
      

        static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionHandler.HandleException(e.Exception);
            e.Handled = true;
        }
        Frame rootFrame;
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            UnhandledException += App_UnhandledException;
            rootFrame = Window.Current.Content as Frame;
            Xamarin.Forms.Forms.Init(e);

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.CacheSize = 1;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                    }
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }
                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                Color color = new Color();
                color.A = 255; color.R = 0; color.G = 168; color.B = 198;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = Colors.Blue;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
                LoginPage.IsLogged = LoggedUser.Instance.IsAuthenticated;
                if (LoggedUser.Instance.CanLogin && LoggedUser.Instance.AutoLogin)
                {
                    TryLoginAsync();
                }
                else if (LoggedUser.Instance.CanLoginByMail && LoggedUser.Instance.AutoLogin)
                {
                    MailLoginAsync();
                }
                else {
                    if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
            }
            Window.Current.Activate();
        }
        private Frame CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                Window.Current.Content = rootFrame;
            }
            return rootFrame;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            MainController.Instance.CollapseWebView();

            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;

                (Window.Current.Content as Frame).Navigate(typeof(LaunchPage), protocolArgs);

                if (LoggedUser.Instance.IsAuthenticated)
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                else
                    (Window.Current.Content as Frame).Navigate(typeof(LoginPage));
            }
        }
        private async Task TryLoginAsync()
        {

            Frame rootFrame = Window.Current.Content as Frame;
            try
            {
                await LoggedUser.Instance.LoginAsync();
            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                if (!rootFrame.Navigate(typeof(LoginPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
                return;
            }
            catch (Exception error)
            {
                if (!rootFrame.Navigate(typeof(LoginPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
                return;
            }
            if (LoggedUser.Instance.IsAuthenticated && !ShareMode)
            {
                if (!rootFrame.Navigate(typeof(MainPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        private async void MailLoginAsync()
        {
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
            }
        }

#if WINDOWS_PHONE_APP
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
        public void RedirectToLoginView(ValidationError error = null)
        {
        }

        public void RedirectToMainView()
        {
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }

        public void RedirectToLoginByEmail(string redirection)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
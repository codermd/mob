using Mxp.Core.Business;
using Mxp.Win.Helpers.SchemeActions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mxp.Win
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LaunchPage : Page, StatesMediator.IStateListener
    {
        public LaunchPage()
        {
            this.InitializeComponent();
            this.mStatesMediator = new StatesMediator(new CommandsFactory(), this);
        }
        private StatesMediator mStatesMediator;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
            base.OnNavigatedTo(e);
            ProtocolActivatedEventArgs protocolArgs = e.Parameter as ProtocolActivatedEventArgs;

            //MainController.Instance.CollapseWebView();
            if (protocolArgs == null)
                this.mStatesMediator.ChangeState(null);
            else if (protocolArgs.Kind == ActivationKind.Protocol)
            {
                if (protocolArgs.Uri != null)
                {
                    UriBuilder build = new UriBuilder(protocolArgs.Uri);
                    if (String.IsNullOrWhiteSpace(protocolArgs.Uri.Host))
                        build.Host = "mxpsessionsharedkey";
                    Uri = build.ToString();
                    this.mStatesMediator.ChangeState(Uri);
                }
                else
                    this.mStatesMediator.ChangeState("");
            }
            //MainController.Instance.CollapseWebView();
            

        }
        String Uri;

       public void RedirectToLoginView(ValidationError error = null)
        {
            (Window.Current.Content as Frame).Navigate(typeof(LoginPage));
        }

        public void RedirectToMainView()
        {
            (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }

        public void RedirectToLoginByEmail(string redirection)
        {
        }
    }
}

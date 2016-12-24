using Mxp.Core.Business;
using Mxp.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Mxp.Win.Helpers
{
    class SAMLCommand : SAMLAbstractCommand
    {

        public SAMLCommand(Uri uri) : base(uri)
        {
        }
        public override void Parse(Uri uri)
        {
            Dictionary<String, String> parameters = HttpUtility.ParseQueryString(System.Net.WebUtility.UrlDecode(uri.Query));

            if (!parameters.ContainsKey("MXPSessionSharedKey"))
                throw new ValidationError("Error", "Wrong scheme");

            this.Token = parameters["MXPSessionSharedKey"];
        }

        public async override void RedirectToLoginView(ValidationError error = null)
        {
            //if (this.NextCommand != null)
            //    await this.NextCommand.InvokeAsync();
            //else {
            //    (Window.Current.Content as Frame).Navigate(typeof(LoginPage));
            //}
        }
        protected async override Task RedirectAsync()
        {
            if (this.NextCommand != null)
                try {
                    await this.NextCommand.InvokeAsync();
                } catch (Exception error) {
                    MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage ());
                    messageDialog.Commands.Add (new UICommand ("OK", (command) => { }));
                    messageDialog.ShowAsync ();
                }
            else
                (Window.Current.Content as Frame).Navigate(typeof(MainPage));
        }
    }
}

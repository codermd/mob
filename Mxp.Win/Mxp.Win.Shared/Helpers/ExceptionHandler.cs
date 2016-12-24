using Caliburn.Micro;
using Mxp.Core.Business;
using Mxp.Core.Services;
using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Mxp.Win
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// Handles failure for application exception on UI thread (or initiated from UI thread via async void handler)
        /// </summary>
        public static async void HandleException(Exception e)
        {
            try {
                await SystemService.Instance.LogExceptionAsync(e);
            }
            catch(Exception er)
            {
                Debug.WriteLine(er.Message);
            }

            Execute.OnUIThread(async () =>
            {
                MessageDialog messageDialog = new MessageDialog("An error occured on the app when processing your request.\n\n" +
                  "Technical support has automatically already been notified. The problem will be solved as soon as possible, in the worst case in the next update.\n\n" +
                  "We appologize for the inconvenience and thank you for your understanding");
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) =>
                {
                    LoggedUser.Instance.ResetData();
                    Application.Current.Exit();
                }));
                await messageDialog.ShowAsync();
            });

        }

        public static string GetExceptionMessage(this Exception e)
        {
            if (e is ValidationError)
                return ((ValidationError) e).Verbose;

            return Service.NoConnectionError;
        }
    }
}
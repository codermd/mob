using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;

namespace Mxp.Win
{
    class PopMessages
    {
        public static void AsyncMessage(String Message)
        {
            MessageDialog messageDialog = new MessageDialog(Message);
            messageDialog.Commands.Add(new UICommand("OK", (command) => { }));
            messageDialog.ShowAsync();
        }
    }
}

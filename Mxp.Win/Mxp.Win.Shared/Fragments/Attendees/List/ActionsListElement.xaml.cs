using Mxp.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class ActionsListElement : UserControl
    {
        public ActionsListElement(Actionable action)
        {
            this.InitializeComponent();
            Action = action;
            TextAction.Text = Action.Title;
        }
        public Actionable Action { get; set; }
        public String Title { get; set; }
        private void TextAction_Click(object sender, RoutedEventArgs e)
        {
            Action.Action();
        }
    }
}

using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Mxp.Win
{
    public sealed partial class HCPListElement : UserControl
    {
        public HCPListElement()
        {
            this.InitializeComponent();
        }
        public Attendee Attendee { get; set; }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine(this.DataContext.ToString());
        }
    }
}

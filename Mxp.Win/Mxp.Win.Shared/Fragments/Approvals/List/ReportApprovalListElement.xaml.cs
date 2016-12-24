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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Mxp.Win
{
    public sealed partial class ReportApprovalListElement : UserControl
    {
        public ReportApprovalListElement()
        {
            this.InitializeComponent();
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            if (ReportApproval != null)
                ((Frame)Window.Current.Content).Navigate(typeof(ApprovalReportDetailView), ReportApproval);
        }

        public ReportApproval ReportApproval { get { return this.DataContext as ReportApproval; } }
    }
}

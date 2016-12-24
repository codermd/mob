using Mxp.Core.Business;
using System;
using System.Collections.Generic;
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
    public sealed partial class DraftReportListElement : UserControl
    {
        public DraftReportListElement()
        {
            this.InitializeComponent();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Report.IsDraft)
                ((Frame)Window.Current.Content).Navigate(typeof(DraftReportDetailView), Report);
            else if (Report.IsOpen)
                ((Frame)Window.Current.Content).Navigate(typeof(OpenReportDetailView), Report);
        }
        public Report Report { get { return this.DataContext as Report; } }

        private void ImageReportStatus_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext as Report).IsDraft)
            {
                (sender as Image).Visibility = Visibility.Collapsed;
            }
        }

        private void ImageReportColor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext as Report).IsDraft)
            {
                (sender as Image).Visibility = Visibility.Collapsed;
            }
        }
    }
}

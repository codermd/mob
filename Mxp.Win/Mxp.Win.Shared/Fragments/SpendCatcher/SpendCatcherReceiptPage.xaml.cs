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
    public sealed partial class SpendCatcherReceiptPage : Page
    {
        public SpendCatcherReceiptPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Receipt);
            InitializeNavigationHelper();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            this.ReceiptImage.Source = e.Parameter as BitmapImage;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
          
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
           
        }
        private void InitializeNavigationHelper()
        {
            navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }
    }
}

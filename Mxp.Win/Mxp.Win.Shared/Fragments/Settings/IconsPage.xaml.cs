using Mxp.Core.Business;
using Mxp.Core.Helpers;
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
    public sealed partial class IconsPage : Page
    {
        public IconsPage()
        {
            this.InitializeComponent();
            this.Title.Text = Labels.GetLoggedUserLabel(Labels.LabelEnum.IconsLegend);
            IconsLegendList = IconsLegend.All;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            ExpensesHeader.Text = IconsLegendList[0].Title;
            ExpensesLegendSource.Source = IconsLegendList[0].IconsLegendList;
            ReportsHeader.Text = IconsLegendList[1].Title;
            ReportsLegendSource.Source = IconsLegendList[1].IconsLegendList; 
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
      

        private List<IconsLegend> IconsLegendList;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}

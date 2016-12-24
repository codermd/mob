using Mxp.Core;
using Mxp.Core.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
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
    public sealed partial class SpendCatcherSection : UserControl
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="image"></param>
        public SpendCatcherSection(SpendCatcherExpense exp, BitmapImage image)
        {
            this.InitializeComponent();

            SpendCatcherExpense = exp;
            this.ImageHolder.Source = image;
            if (Fields == null)
                Fields = SpendCatcherExpense.Fields;
            FieldsListView.ItemsSource = Fields;
            GridSection.Width = (Window.Current.Content as Frame).ActualWidth;
         
        }
        public SpendCatcherExpense SpendCatcherExpense;
        public Collection<Field> Fields { get; set; }

    }
}

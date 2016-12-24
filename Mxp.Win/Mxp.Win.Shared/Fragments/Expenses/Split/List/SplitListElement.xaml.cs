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
using Windows.UI.Xaml.Navigation;



namespace Mxp.Win
{
    public sealed partial class SplitListElement : UserControl
    {
        public SplitListElement()
        {
            this.InitializeComponent();
        }
        public event EventHandler DeleteInnerRequest;
        public event EventHandler EditInnerRequest;
        internal void EditInner()
        {
            if (EditInnerRequest != null)
                EditInnerRequest(this, EventArgs.Empty);
        }
        internal void DeleteInner()
        {
            if (DeleteInnerRequest != null)
                DeleteInnerRequest(this, EventArgs.Empty);
        }
        private void Edit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditInner();
        }
        private void Delete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DeleteInner();
        }
    }
}

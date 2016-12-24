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
    public sealed partial class ATextBlock : UserControl
    {
        public ATextBlock(Field field)
        {
            this.InitializeComponent();
            CellField = field;
            SetVisual();
        }
        public void SetVisual()
        {
            if(CellField.VTitle!=null)
                this.TextBlockField.Text = CellField.VTitle;
        }
        public Field CellField { get; set; }
    }
}


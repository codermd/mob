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
    public sealed partial class APolicyRule : UserControl
    {
        public APolicyRule(Field field)
        {
            this.InitializeComponent();
            CellField = field;
            Message = CellField.extraInfo["Message"] as string;
            PolicyExpenseToBitmapConverter conv = new PolicyExpenseToBitmapConverter();
            ExpenseItem.PolicyRules p = (ExpenseItem.PolicyRules) CellField.extraInfo["Icon"];
            this.ImagePolicyRule.Source = conv.Convert(p, typeof(BitmapImage), null, null) as BitmapImage;
            this.TextBlockField.Text = Message;
            
            
        }
        public void SetVisual()
        {
            
          
        
        }
        public Field CellField { get; set; }
        public String Message { get; set; }
    }
}


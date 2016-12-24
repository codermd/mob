using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Mxp.Win
{
    public class DebugListView : ListView
    {
        public int GetContainerCount { get; set; }
        public int PrepareContainerCount { get; set; }

        protected override Windows.UI.Xaml.DependencyObject GetContainerForItemOverride()
        {
            GetContainerCount++;
            return base.GetContainerForItemOverride();
        }

        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            PrepareContainerCount++;
            base.PrepareContainerForItemOverride(element, item);
        }
    }
}


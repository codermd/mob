
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mxp.Win
{
    public class SelectionHub : Hub
    {
        private bool _settingIndex;

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        public int VisibleSectionsCount
        {
            get
            {
                int count = 0;
                foreach (HubSection section in Sections)
                {
                    if (section.Visibility == Visibility.Visible)
                        count++;
                }
                return count;
            }
        }
        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(SelectionHub),
            new PropertyMetadata(0, OnSelectedIndexChanged));

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var scroller = GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (scroller == null) return;

            scroller.ViewChanged += ScrollerOnViewChanged;
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hub = d as SelectionHub;
            if (hub == null) return;
            if (hub._settingIndex) return;
            if (hub.Sections.Count == 0) return;

            var section = hub.Sections[hub.SelectedIndex];
            hub.ScrollToSection(section);
            
        }

        private void ScrollerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs scrollViewerViewChangedEventArgs)
        {
            _settingIndex = true;
            //SelectedIndex = Sections.IndexOf(SectionsInView[0]);
            ScrollViewer scrollViewer = sender as ScrollViewer;
                        
            if (scrollViewer.HorizontalOffset > (scrollViewer.ViewportWidth / 2))
                SelectedIndex = 1;
            else
                SelectedIndex = 0;

            //if (scrollViewer.HorizontalOffset <= scrollViewer.ViewportWidth / Sections.Count)
            //{
            //    SelectedIndex = 0;

            //}
            //else
            //{
            //SelectedIndex = (int)(scrollViewer.HorizontalOffset / ((scrollViewer.ViewportWidth / 2) + (scrollViewer.ViewportWidth * SelectedIndex)));

            //}


            _settingIndex = false;
            SelectedIndexChanged();
        }

        public event EventHandler SelectedIndexChangedRequest;
        public void SelectedIndexChanged()
        {
            if (SelectedIndexChangedRequest != null)
                SelectedIndexChangedRequest(this, EventArgs.Empty);
        }


    }
}

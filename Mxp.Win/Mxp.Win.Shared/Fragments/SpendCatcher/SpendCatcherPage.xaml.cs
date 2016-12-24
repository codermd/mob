using Mxp.Core;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class SpendCatcherPage : Page
    {
        ShareOperation shareOperation;
        private IReadOnlyList<IStorageItem> m_sharedStorageItems;
        string Base64str;
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("Sections"))
                Sections = e.PageState["Sections"] as Collection<SpendCatcherSection>;
            if (e.PageState != null && e.PageState.ContainsKey("ScrollOffset"))
                ScrollOffset = (double)e.PageState["ScrollOffset"];
            if (e.PageState != null && e.PageState.ContainsKey("AcceptButtonActivated"))
                AcceptButtonActivated = (bool)e.PageState["AcceptButtonActivated"];
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (Sections != null)
                e.PageState["Sections"] = Sections;
            if (ScrollOffset != null)
                e.PageState["ScrollOffset"] = ScrollOffset;
            if (StackSpendCatchers != null)
                e.PageState["StackSpendCatchers"] = StackSpendCatchers;
            if (AcceptButtonActivated != null)
                e.PageState["AcceptButtonActivated"] = AcceptButtonActivated;
        }
        private void InitializeNavigationHelper()
        {
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public SpendCatcherPage()
        {
            this.InitializeComponent();
            Sections = new Collection<SpendCatcherSection>();
            this.navigationHelper = new NavigationHelper(this);
            InitializeNavigationHelper();
            ScrollOffset = 0;
            ScrollPanel.HorizontalSnapPointsType = SnapPointsType.Mandatory;
            Title.Text = LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcher);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ScrollOffset = ScrollPanel.HorizontalOffset;
            navigationHelper.OnNavigatedFrom(e);
            this.StackSpendCatchers.Children.Clear();
            ScrollPanel.ViewChanged -= ScrollPanel_ViewChanged;
            StackSpendCatchers.Loaded -= StackSpendCatchers_Loaded;
            ScrollPanel.HorizontalSnapPointsType = SnapPointsType.Mandatory;
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            StackSpendCatchers.Loaded += StackSpendCatchers_Loaded;
            ScrollPanel.HorizontalSnapPointsType = SnapPointsType.Mandatory;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
          
            if (Sections.Count == 0)
            {
                this.shareOperation = (ShareOperation)e.Parameter;
                if (shareOperation == null)
                    return;
                if (this.shareOperation.Data.Contains(StandardDataFormats.StorageItems) && StackSpendCatchers.Children.Count == 0)
                {
                    try
                    {
                        m_sharedStorageItems = await shareOperation.Data.GetStorageItemsAsync();
                        List<String> Items = new List<String>();
                        foreach (IStorageFile item in m_sharedStorageItems)
                        {
                            var refe = RandomAccessStreamReference.CreateFromFile(item);
                            Base64str = await BitmapHelper.CompressFile(item as StorageFile);
                            var image = new BitmapImage();
                            using (var stream = await refe.OpenReadAsync())
                                image.SetSource(stream);
                            SpendCatcherExpense exp = new SpendCatcherExpense(Base64str);
                            SpendCatcherSection section = new SpendCatcherSection(exp, image);
                            Sections.Add(section);
                        }
                        foreach (var sec in Sections)
                            this.StackSpendCatchers.Children.Add(sec);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed GetStorageItemsAsync - " + ex.Message);
                    }
                }
            }
            else
            {
                foreach (var section in Sections)
                    this.StackSpendCatchers.Children.Add(section);
            }

            if (Sections.Count == 1 || AcceptButtonActivated)
                ActivateAcceptButton();
         



        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Application.Current.Exit();
        }

        bool AcceptButtonActivated = false;
        private void ActivateAcceptButton()
        {
            this.NextButton.Visibility = Visibility.Collapsed;
            this.AcceptButton.Visibility = Visibility.Visible;
            AcceptButtonActivated = true;
        }

        Collection<SpendCatcherSection> Sections;
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            SpendCatcherExpenses expenses = new SpendCatcherExpenses();
            foreach (var sec in Sections)
                expenses.Add(sec.SpendCatcherExpense);
            try
            {
                ActivateProgressRing();
                await expenses.SendAsync();
                 
                DesactivateProgressRing();
                MessageDialog messageDialog = new MessageDialog(LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcherConfirmationMessage));
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Close)), (command) => { LeaveApp(); }));
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.SpendCatcherOpenApp)), (command) => { GoToMainPage(); }));
                messageDialog.ShowAsync();

            }
            catch (ValidationError error)
            {
                MessageDialog messageDialog = new MessageDialog(error.Verbose);
                messageDialog.Commands.Add(new UICommand((LoggedUser.Instance.Labels.GetLabel(Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync();
                DesactivateProgressRing();
                return;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                DesactivateProgressRing();
                return;
            }
        }

        private void LeaveApp()
        {
            Application.Current.Exit();
        }

        private void GoToMainPage()
        {
            Frame.GoBack();
            MainController.Instance.refreshButton();
        }

        double ScrollOffset;
        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            ScrollOffset = ScrollPanel.HorizontalOffset;
            ScrollPanel.ChangeView(ScrollOffset + (int)GridPage.ActualWidth, null, null);

            if (ScrollOffset > ((Sections.Count - 2) * (int)GridPage.ActualWidth) - 50)
            {
                ActivateAcceptButton();
            }
        }

        private void ScrollPanel_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollOffset = ScrollPanel.HorizontalOffset;
            if (ScrollOffset > ((Sections.Count - 2) * (int)GridPage.ActualWidth) - 50)
            {
                ActivateAcceptButton();
            }
        }

        private void StackSpendCatchers_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollPanel.ChangeView(ScrollOffset, null, null, false);
            ScrollPanel.ViewChanged += ScrollPanel_ViewChanged;
        }
        private void ActivateProgressRing()
        {
            ProgressRing.IsActive = true;
            BottomAppBar.IsEnabled = false;
            ProgressGrid.Visibility = Visibility.Visible;
        }
        private void DesactivateProgressRing()
        {
            ProgressRing.IsActive = false;
            BottomAppBar.IsEnabled = true;
            ProgressGrid.Visibility = Visibility.Collapsed;

        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}

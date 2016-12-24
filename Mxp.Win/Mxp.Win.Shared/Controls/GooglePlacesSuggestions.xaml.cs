using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mxp.Core.Business;
using Mxp.Core.Services;
using Mxp.Core.Services.Google;

namespace Mxp.Win.Controls
{
    public sealed partial class GooglePlacesSuggestions
    {
        private Field _field;

        public delegate void SelectedEventHandler (object sender, SelectedEventArgs e);

        public event SelectedEventHandler Selected;

        private List<Prediction> _predictions;
        public GooglePlacesSuggestions()
        {
            this.InitializeComponent();
        }

        private async void SuggestionTbTextChanged (AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
                return;

            try {
                this._predictions = (await GoogleService.Instance.FetchPlacesLocationsAsync (this.SuggestionTB.Text, GoogleService.PlaceTypeEnum.Merchant,this._field.GetModel<ExpenseItem> ()?.Country)).predictions;
                PredictionResultItem packagesResult = new PredictionResultItem ();

                packagesResult.items.AddRange (this._predictions.Select (d => new PredictionResultItem.Item (d)));
                sender.ItemsSource = packagesResult.items;
            }
            catch (Exception error) {
                MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage());
                messageDialog.Commands.Add (new UICommand ((LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync ();
            }
        }
        private void SearchTB_SuggestionChosen (AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
            Prediction item = ((PredictionResultItem.Item)args.SelectedItem).PredictionItem;
            try {
                this.SuggestionTB.TextChanged -= this.SuggestionTbTextChanged;
                this._field.Value = item;
                this.Selected?.Invoke (this, new SelectedEventArgs (item));
                this.Frame.GoBack ();
            }
            catch (Exception error) {
                MessageDialog messageDialog = new MessageDialog (error.GetExceptionMessage());
                messageDialog.Commands.Add (new UICommand ((LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Accept)), (command) => { }));
                messageDialog.ShowAsync ();
            }
        }

        void HardwareButtons_BackPressed (object sender, BackPressedEventArgs e) {
            e.Handled = true;
            this.Frame.GoBack ();
        }
        protected override void OnNavigatedFrom (NavigationEventArgs e) {
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
        }
        protected override void OnNavigatedTo (NavigationEventArgs e) {
            this.Title.Text = (LoggedUser.Instance.Labels.GetLabel (Labels.LabelEnum.Select));
            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
            if (e.Parameter is Field) {
                this._field = (Field) e.Parameter;

                Prediction prediction = this._field.Value as Prediction;

                if (!String.IsNullOrEmpty (prediction?.description)) {
                    this.SuggestionTB.Text = prediction.description;
                }
            }
        }
    }
}
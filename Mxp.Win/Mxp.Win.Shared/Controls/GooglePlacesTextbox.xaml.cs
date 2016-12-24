using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Mxp.Core.Business;
using Mxp.Core.Services.Google;

namespace Mxp.Win.Controls
{
    public sealed partial class GooglePlacesTextbox
    {
        private readonly Field _field;

        public GooglePlacesTextbox(Field field)
        {
            this.InitializeComponent();

            this._field = field;

            this.SearchTB.Text = this._field.VValue ?? string.Empty;
        }

        private void SearchTB_OnTapped (object sender, TappedRoutedEventArgs e) {
            ((Frame)Window.Current.Content).Navigate (typeof (GooglePlacesSuggestions), this._field);
        }
    }

    public class SelectedEventArgs : EventArgs
    {
        public Prediction Prediction { get; private set; }

        public SelectedEventArgs (Prediction prediction) {
            this.Prediction = prediction;
        }
    }
}
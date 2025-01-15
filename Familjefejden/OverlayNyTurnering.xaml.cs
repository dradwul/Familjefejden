using Familjefejden.Service;
using Klasser;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Familjefejden
{
    public sealed partial class OverlayNyTurnering : Page
    {
        TurneringService turneringService = new TurneringService();
        JsonService jsonService = new JsonService();

        public OverlayNyTurnering()
        {
            this.InitializeComponent();
            InitializePage();
        }

        private void InitializePage()
        {
            KollaTurneringInputIkon.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void NastaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            if (TurneringsLista.SelectedItem is ComboBoxItem selectedItem)
            {
                string turneringensNamn = selectedItem.Content.ToString();
                Turnering nyTurnering = turneringService.SkapaTurnering(turneringensNamn);

                bool sparad = await jsonService.LaggaTillNyTurneringAsync(nyTurnering);

                if (sparad)
                {
                    Frame.Navigate(typeof(OverlayLaggaTillLag));
                }
                else
                {
                    // Denna ska ändras om fler turneringar ska finnas
                    Frame.Navigate(typeof(OverlayLaggaTillLag));
                    //var errorDialog = new ContentDialog
                    //{
                    //    Title = "Fel",
                    //    Content = "Kunde inte skapa turneringen. Försök igen.",
                    //    CloseButtonText = "Ok",
                    //    CornerRadius = new CornerRadius(10)
                    //};
                    //await errorDialog.ShowAsync();
                }
            }
        }

        private void TurneringsLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TurneringsLista.SelectedItem is ComboBoxItem selectedItem)
            {
                bool isValid = selectedItem.Content.ToString() == "JVM";
                NastaKnapp.IsEnabled = isValid;
                KollaTurneringInputIkon.Foreground = new SolidColorBrush(
                    isValid ? Windows.UI.Colors.LightGreen : Windows.UI.Colors.Gray);
            }
            else
            {
                NastaKnapp.IsEnabled = false;
                KollaTurneringInputIkon.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
            }
        }
    }
}
using Familjefejden.Service;
using Klasser;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Familjefejden
{
    public sealed partial class OverlayHanteraGrupp : Page
    {
        GruppService gruppService = new GruppService();
        JsonService jsonService = new JsonService();
        private List<Anvandare> spelarLista = new List<Anvandare>();
        private List<Anvandare> nyaSpelareAttSpara = new List<Anvandare>();
        private bool finnsGrupp;

        public OverlayHanteraGrupp()
        {
            this.InitializeComponent();
            LaddaInBefintligData();
        }

        private async void LaddaInBefintligData()
        {
            finnsGrupp = await jsonService.KollaOmDetRedanFinnsEnGrupp();
            if (finnsGrupp)
            {
                GruppnamnLabel.Text = "Redigerar grupp";
                NyGrupp.Text = await jsonService.HamtaGruppnamnFranId(1);
                NyGrupp.IsEnabled = false;
            }

            spelarLista = await jsonService.HamtaAllaAnvandareAsync();
            SpelarListView.ItemsSource = spelarLista;
        }

        private void NySpelare_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                NySpelare_Klickad(sender, e);
            }
        }

        private void NySpelare_Klickad(object sender, RoutedEventArgs e)
        {
            var nySpelare = NySpelare.Text.Trim();
            if (!string.IsNullOrEmpty(nySpelare))
            {
                var nySpelareAttSpara = gruppService.SkapaAnvandare(nySpelare);
                nyaSpelareAttSpara.Add(nySpelareAttSpara);

                spelarLista.Add(nySpelareAttSpara);
                SpelarListView.ItemsSource = null;
                SpelarListView.ItemsSource = spelarLista;

                NySpelare.Text = string.Empty;
            }
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            finnsGrupp = await jsonService.KollaOmDetRedanFinnsEnGrupp();

            if (!finnsGrupp)
            {
                string nyGruppNamn = NyGrupp.Text.Trim();
                if (!string.IsNullOrEmpty(nyGruppNamn))
                {
                    Grupp nyGrupp = gruppService.SkapaGrupp(nyGruppNamn);
                    await jsonService.LaggTillNyGruppAsync(nyGrupp);
                }

                finnsGrupp = await jsonService.KollaOmDetRedanFinnsEnGrupp();
            }

            if (finnsGrupp)
            {
                foreach (var spelare in nyaSpelareAttSpara)
                {
                    await jsonService.LaggTillAnvandareIGruppAsync(spelare);
                }
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "Fel",
                    Content = "Kunde inte skapa grupp.",
                    CloseButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }
        }

        private void NySpelare_SelectionChanged(object sender, RoutedEventArgs e)
        {
            LaggTillSpelareKnapp.IsEnabled = !string.IsNullOrEmpty(NySpelare.Text);
        }

        private void NyGrupp_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NyGrupp.Text))
            {
                KollaGruppInputIkon.Foreground = new SolidColorBrush(Windows.UI.Colors.LightGreen);
            }
            else
            {
                KollaGruppInputIkon.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
            }   
        }

        private async void TaBortSpelare_Klickad(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag != null)
            {
                int spelarId = (int)button.Tag;
                var spelareAttTaBort = spelarLista.FirstOrDefault(s => s.Id == spelarId);

                if (spelareAttTaBort != null)
                {
                    var bekraftaDialog = new ContentDialog
                    {
                        Title = "Bekräfta borttagning",
                        Content = $"Är du säker på att du vill ta bort {spelareAttTaBort.Namn}?",
                        PrimaryButtonText = "Ja",
                        CloseButtonText = "Avbryt",
                        DefaultButton = ContentDialogButton.Close
                    };

                    var result = await bekraftaDialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        bool borttagen = await jsonService.TaBortAnvandareIGruppAsync(spelarId);

                        if (borttagen)
                        {
                            spelarLista.Remove(spelareAttTaBort);
                            SpelarListView.ItemsSource = null;
                            SpelarListView.ItemsSource = spelarLista;
                        }
                        else
                        {
                            var errorDialog = new ContentDialog
                            {
                                Title = "Fel",
                                Content = "Kunde inte ta bort spelaren.",
                                CloseButtonText = "Ok"
                            };
                            await errorDialog.ShowAsync();
                        }
                    }
                }
            }
        }
    }
}

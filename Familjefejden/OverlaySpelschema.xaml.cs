using Familjefejden.Klasser;
using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class OverlaySpelschema : Page
    {
        TurneringService turneringService = new TurneringService();
        JsonService jsonService = new JsonService();
        private ObservableCollection<MatchForemal> tillagdaMatcher;

        public OverlaySpelschema()
        {
            this.InitializeComponent();
            this.Loaded += OverlaySpelschema_Loaded;

            tillagdaMatcher = new ObservableCollection<MatchForemal>();
            ListaSpelschema.ItemsSource = tillagdaMatcher;
        }

        private void OverlaySpelschema_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var lagBildHemma = new List<LagForemal>
            {
                new LagForemal { LagFlagga = "ms-appx:///Assets/Canada.png", Lag = "Kanada" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Czech_Republic.png", Lag = "Tjeckien" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Finland.png", Lag = "Finland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Germany.png", Lag = "Tyskland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Kazakhstan.png", Lag = "Kazakhstan" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Latvia.png", Lag = "Lettland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Slovakia.png", Lag = "Slovakien" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Sweden.png", Lag = "Sverige" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Switzerland.png", Lag = "Schweiz" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/USA.png", Lag = "USA" }
            };
            var lagBildBorta = new List<LagForemal>
            {
                new LagForemal { LagFlagga = "ms-appx:///Assets/Canada.png", Lag = "Kanada" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Czech_Republic.png", Lag = "Tjeckien" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Finland.png", Lag = "Finland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Germany.png", Lag = "Tyskland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Kazakhstan.png", Lag = "Kazakhstan" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Latvia.png", Lag = "Lettland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Slovakia.png", Lag = "Slovakien" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Sweden.png", Lag = "Sverige" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Switzerland.png", Lag = "Schweiz" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/USA.png", Lag = "USA" }
            };

            MatchListaHemma.ItemsSource = lagBildHemma;
            MatchListaBorta.ItemsSource = lagBildBorta;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayLaggaTillLag));
        }

        private void LaggTillMatchKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            //TODO: Logik för att lägga till lag i listview
            //TODO: Logik för att spara match i json
            //Match nyMatch = turneringService.SkapaMatch(datum, hemmalagId, bortalagId);
            //jsonService.LaggTillMatchAsync(nyMatch);

            //var dialog = new MessageDialog("Laget är redan tillagt.");
            //await dialog.ShowAsync();

            //var dialog = new MessageDialog("Du kan inte lägga till fler än 10 lag.");
            //await dialog.ShowAsync();

            //Lag nyttLag = turneringService.SkapaLag(valtForemal.Lag);
            //await jsonService.LaggTillLagAsync(nyttLag);

            var valtForemalHemma = MatchListaHemma.SelectedItem as LagForemal;
            var valtForemalBorta = MatchListaBorta.SelectedItem as LagForemal;

            if (valtForemalHemma != null && valtForemalBorta != null)
            {
                var datum = MatchDag.Date.DateTime.Date; // Endast datumdelen
                var tid = MatchStart.Time;

                tillagdaMatcher.Add(new MatchForemal
                {
                    HemmaLagFlagga = valtForemalHemma.LagFlagga,
                    HemmaLag = valtForemalHemma.Lag,
                    BortaLagFlagga = valtForemalBorta.LagFlagga,
                    BortaLag = valtForemalBorta.Lag,
                    Datum = datum,
                    Tid = tid
                });
            }
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void TabortKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var knapp = sender as Button;
            var matchForemal = knapp.DataContext as MatchForemal;
            if (matchForemal != null)
            {
                tillagdaMatcher.Remove(matchForemal);
            }
        }

        public class MatchForemal
        {
            public string HemmaLagFlagga { get; set; }
            public string HemmaLag { get; set; }
            public string BortaLagFlagga { get; set; }
            public string BortaLag { get; set; }
            public DateTime Datum { get; set; }
            public TimeSpan Tid { get; set; }
        }
    }
}

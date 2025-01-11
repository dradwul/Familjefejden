using Familjefejden.Klasser;
using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
        List<Match> listaMedMatcherSomSkaSparas = new List<Match>();

        public OverlaySpelschema()
        {
            this.InitializeComponent();
            //HamtaAllaLagFranDatabas();
            this.Loaded += OverlaySpelschema_Loaded;

            tillagdaMatcher = new ObservableCollection<MatchForemal>();
            ListaSpelschema.ItemsSource = tillagdaMatcher;
        }

        private void OverlaySpelschema_Loaded(object sender, RoutedEventArgs e)
        {
            var lagBildHemma = LagForemal.HamtaLagForemal();
            var lagBildBorta = LagForemal.HamtaLagForemal();

            Hemmalag.ItemsSource = lagBildHemma;
            Bortalag.ItemsSource = lagBildBorta;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayLaggaTillLag));
        }

        private async void LaggTillMatchKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var valtLagHemma = Hemmalag.SelectedItem as LagForemal;
            var valtLagBorta = Bortalag.SelectedItem as LagForemal;

            if(valtLagHemma != null && valtLagBorta != null)
            {
                var datum = MatchDag.Date.DateTime.Date;
                var tid = MatchStart.Time;
                var matchDatum = new DateTime(datum.Year, datum.Month, datum.Day, tid.Hours, tid.Minutes, 0);

                int hemmalagId = await jsonService.HamtaLagIdFranNamn(valtLagHemma.Lag);
                int bortalagId = await jsonService.HamtaLagIdFranNamn(valtLagBorta.Lag);

                if(hemmalagId == -1 || bortalagId == -1)
                {
                    var dialog = new MessageDialog("Ett av lagen kunde inte hittas");
                    await dialog.ShowAsync();
                    return;
                }

                Match nyMatch = turneringService.SkapaMatch(matchDatum, hemmalagId, bortalagId);
                listaMedMatcherSomSkaSparas.Add(nyMatch);

                tillagdaMatcher.Add(new MatchForemal
                {
                    HemmaLagFlagga = valtLagHemma.LagFlagga,
                    HemmaLag = valtLagHemma.Lag,
                    BortaLagFlagga = valtLagBorta.LagFlagga,
                    BortaLag = valtLagBorta.Lag,
                    Datum = datum,
                    Tid = tid
                });

                Hemmalag.SelectedItem = null;
                Bortalag.SelectedItem = null;

                AccepteraKnapp.IsEnabled = true;
            }
            else
            {
                var dialog = new MessageDialog("Vänligen välj både hemmalag och bortalag.");
                await dialog.ShowAsync();
            }
           

            //TODO: Logik för att lägga till lag i listview
            //TODO: Logik för att spara match i json
            //Match nyMatch = turneringService.SkapaMatch(Datum, hemmalagId, bortalagId);
            //jsonService.LaggTillMatchAsync(nyMatch);

            //var dialog = new MessageDialog("Laget är redan tillagt.");
            //await dialog.ShowAsync();

            //var dialog = new MessageDialog("Du kan inte lägga till fler än 10 lag.");
            //await dialog.ShowAsync();

            //Lag nyttLag = turneringService.SkapaLag(valtForemal.Lag);
            //await jsonService.LaggTillLagAsync(nyttLag);

            //var valtForemalHemma = Hemmalag.SelectedItem as LagForemal;
            //var valtForemalBorta = Bortalag.SelectedItem as LagForemal;

            //if (valtForemalHemma != null && valtForemalBorta != null)
            //{
            //    var datum = MatchDag.Date.DateTime.Date; // Endast datumdelen
            //    var tid = MatchStart.Time;

            //    tillagdaMatcher.Add(new MatchForemal
            //    {
            //        HemmaLagFlagga = valtForemalHemma.LagFlagga,
            //        HemmaLag = valtForemalHemma.Lag,
            //        BortaLagFlagga = valtForemalBorta.LagFlagga,
            //        BortaLag = valtForemalBorta.Lag,
            //        Datum = datum,
            //        Tid = tid
            //    });
            //}
        }

        private DateTime GetSelectedDateTime()
        {
            var datum = MatchDag.Date.DateTime;
            var tid = MatchStart.Time;

            return new DateTime(datum.Year, datum.Month, datum.Day, tid.Hours, tid.Minutes, 0, DateTimeKind.Utc);
        }

        private async void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            foreach(var match in listaMedMatcherSomSkaSparas)
            {
                bool laggaTillMatch = await jsonService.LaggTillMatchAsync(match);
                if (!laggaTillMatch)
                {
                    var dialog = new MessageDialog("Matchen kunde inte läggas till");
                    await dialog.ShowAsync();
                    return;
                }
            }

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

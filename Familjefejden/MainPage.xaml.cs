using Familjefejden.Klasser;
using Familjefejden.Service;
using Klasser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Familjefejden
{
    public sealed partial class MainPage : Page
    {
        JsonService jsonService = new JsonService();
        GruppService gruppService = new GruppService();
        TurneringService turneringService = new TurneringService();
        List<Match> allaMatcher = new List<Match>();

        public MainPage()
        {
            this.InitializeComponent();
            HamtaDataAsync();
            LaddaDataAsync();
        }

        private async void LaddaDataAsync()
        {
            allaMatcher = await jsonService.HamtaAllaMatcherAsync();
            var matchObjekt = new List<dynamic>();

            foreach (var match in allaMatcher)
            {
                var hemmalagNamn = await jsonService.HamtaLagnamnFranLagId(match.HemmalagId);
                var hemmalagFlagga = LagForemal.HamtaLagForemal()
                    .FirstOrDefault(l => l.Lag == hemmalagNamn)?.LagFlagga;

                var bortalagNamn = await jsonService.HamtaLagnamnFranLagId(match.BortalagId);
                var bortalagFlagga = LagForemal.HamtaLagForemal()
                    .FirstOrDefault(l => l.Lag == bortalagNamn)?.LagFlagga;

                matchObjekt.Add(new
                {
                    HemmalagNamn = hemmalagNamn,
                    BortalagNamn = bortalagNamn,
                    HemmalagFlagga = hemmalagFlagga,
                    BortalagFlagga = bortalagFlagga,
                    HemmalagMal = match.HemmalagMal,
                    BortalagMal = match.BortalagMal,
                    match.Id,
                    MatchDatum = match.Date,
                    Datum = match.Date.ToString("dd/MM/yyyy"),
                    Tid = match.Date.ToString("HH:mm")
                });
            }

            var kommande = matchObjekt
                .Where(m => m.MatchDatum >= DateTime.Today.Date)
                .OrderBy(m => m.MatchDatum);

            var resultat = matchObjekt
                .Where(m => m.MatchDatum < DateTime.Today.Date)
                .OrderByDescending(m => m.MatchDatum);

            foreach (var match in kommande)
            {
                KommandeMatcher.Items.Add(match);
            }

            foreach (var match in resultat)
            {
                ResultatMatcher.Items.Add(match);
            }
        }

        // Denna metod skapar en lokal kopia av datafilen
        // om den inte redan finns
        private async void HamtaDataAsync()
        {
            await jsonService.KopieraFilTillLokalMappAsync("dataFil.json");
        }

        private async void KnappVisaTopplista_Klickad(object sender, RoutedEventArgs e)
        {
            var topplistaPopup = new OverlayTopplista();
            await topplistaPopup.ShowAsync();
        }

        private void NyGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayHanteraGrupp));
        }

        private void NyTurnering_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyTurnering));
        }

        private void BetKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayBetVy));
        }

        private async void ReglerKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var hjalpDialog = new OverlayReglerPoang();
            await hjalpDialog.ShowAsync();
        }

        private void RattaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayRattaMatcher));
        }

        private void AvslutaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}

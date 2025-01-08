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

        public MainPage()
        {
            this.InitializeComponent();
            HamtaDataAsync();
            LaddaData(); //Dummy metod
        }
        // DUMMY METOD för att fylle RESULTAT och KOMMANDE listorna med matcher
        private void LaddaData()
        {
            var allaMatcher = DummyData.GetDummyMatches();
            var avslutadeMatcher = DummyData.GetFinishedMatches();
            var flaggor = DummyData.GetCountryFlags();

            var resultatMatcher = avslutadeMatcher
                .Where(m => m.Date < DateTime.Now)
                .OrderByDescending(m => m.Date.Date)
                .ThenBy(m => m.Date.TimeOfDay)
                .ToList();

            var kommandeMatcher = allaMatcher
                .Where(m => m.Date >= DateTime.Now)
                .OrderBy(m => m.Date.Date)
                .ThenBy(m => m.Date.TimeOfDay)
                .ToList();

            ResultatMatcher.ItemsSource = resultatMatcher.Select(match => new
            {
                match.HemmalagId,
                match.BortalagId,
                Team1Flaggor = flaggor[match.HemmalagId],
                Team2Flaggor = flaggor[match.BortalagId],                
                Datum = match.Date.ToString("dd/MM/yyyy"),
                Tid = match.Date.ToString("HH:mm"),
                ResultatHemma = $"{match.HemmalagMal}",
                ResultatBorta = $"{match.BortalagMal}"
            }).ToList();

            KommandeMatcher.ItemsSource = kommandeMatcher.Select(match => new
            {
                match.HemmalagId,
                match.BortalagId,
                Team1Flaggor = flaggor[match.HemmalagId],
                Team2Flaggor = flaggor[match.BortalagId],
                Datum = match.Date.ToString("dd/MM/yyyy"),
                Tid = match.Date.ToString("HH:mm")
            }).ToList();
        }

        private async void HamtaDataAsync()
        {
            await jsonService.KopieraFilTillLokalMappAsync("dataFil.json");
        }

        private async void KnappVisaTopplista_Klickad(object sender, RoutedEventArgs e)
        {
            var topplistaPopup = new OverlayTopplista();
            await topplistaPopup.ShowAsync();
        }

        private void AvslutaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }



        // // //  TESTKNAPPAR:
        private async void TestaLaggaTillGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            Grupp nyGrupp = gruppService.SkapaGrupp("OOP2-Familjefejden");
            await jsonService.LaggTillNyGruppAsync(nyGrupp);
        }

        private async void TestaLaggaTillTurnering_Klickad(object sender, RoutedEventArgs e)
        {
            Turnering nyTurnering = turneringService.SkapaTurnering("JVM2025");
            await jsonService.LaggaTillNyTurneringAsync(nyTurnering);
        }

        private async void TestaLaggaTillAnvandare_Klickad(object sender, RoutedEventArgs e)
        {
            Anvandare nyAnvandare = gruppService.SkapaAnvandare("TestAnvändare");
            await jsonService.LaggTillAnvandareIGruppAsync(nyAnvandare);
        }

        private async void TestaLaggaTillLag_Klickad(object sender, RoutedEventArgs e)
        {
            Lag nyttLag = turneringService.SkapaLag("TestLag");
            await jsonService.LaggTillLagAsync(nyttLag);
        }

        private async void TestaLaggaTillMatch_Klickad(object sender, RoutedEventArgs e)
        {
            DateTime datum = DateTime.UtcNow;
            Random random = new Random();
            Match nyMatch = turneringService.SkapaMatch(datum, random.Next(0,10), random.Next(0, 10));
            await jsonService.LaggTillMatchAsync(nyMatch);
        }

        private async void TestaLaggaTillBet_Klickad(object sender, RoutedEventArgs e)
        {
            // Testar felhantering med slumpade IDn för användare och match
            Random random = new Random();
            int anvandareId = random.Next(0, 6);
            int matchId = random.Next(0, 10); 
            int hemmaMal = random.Next(0, 8);
            int bortaMal = random.Next(0, 8);
            Debug.WriteLine($"[anvId:{anvandareId}|matchId:{matchId}|resultat:{hemmaMal}-{bortaMal}]");
            Bet nyttBet = gruppService.LaggBetPaMatch(matchId, hemmaMal, bortaMal);
            await jsonService.LaggTillBetAsync(anvandareId, nyttBet);
        }

        private async void LaggTillPoangForAnvandarIdEtt_Klickad(object sender, RoutedEventArgs e)
        {
            await jsonService.UppdateraPoangForAnvandareAsync(1, 25);
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
    }
}

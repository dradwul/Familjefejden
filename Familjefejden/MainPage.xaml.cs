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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Familjefejden
{
    public sealed partial class MainPage : Page
    {        
        public MainPage()
        {
            this.InitializeComponent();
            LaddaData(); //Dummy metod
        }
        // DUMMY METOD för att fylle RESULTAT och KOMMANDE listorna med matcher
        private void LaddaData()
        {
            var allaMatcher = DummyData.GetDummyMatches();
            var avslutadeMatcher = DummyData.GetFinishedMatches();
            var flaggor = DummyData.GetCountryFlags();

            var resultatMatcher = avslutadeMatcher.Where(m => m.Date < DateTime.Now).ToList();
            var kommandeMatcher = allaMatcher.Where(m => m.Date >= DateTime.Now).ToList();      

            ResultatMatcher.ItemsSource = resultatMatcher.Select(match => new
            {
                match.Team1,
                match.Team2,
                Team1Flaggor = flaggor[match.Team1],
                Team2Flaggor = flaggor[match.Team2],                
                Date = match.Date.ToString("dd/MM/yyyy"),
                ResultatHemma = $"{match.Team1Score}",
                ResultatBorta = $"{match.Team2Score}"
            }).ToList();

            KommandeMatcher.ItemsSource = kommandeMatcher.Select(match => new
            {
                match.Team1,
                match.Team2,
                Team1Flaggor = flaggor[match.Team1],
                Team2Flaggor = flaggor[match.Team2],
                Datum = match.Date.ToString("dd/MM/yyyy")
            }).ToList();
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

        private void NyGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyGrupp));
        }

        private void BetKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayBetVy));
        }
    }
}

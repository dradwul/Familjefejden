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
    public sealed partial class OverlayBetVy : Page
    {
        private List<Anvandare> anvandare; //DUMMYDATA För tillfället, ska tas bort senare.
        
        public OverlayBetVy()
        {
            this.InitializeComponent();
            LaddaDummyData();
        }
        
        private void LaddaDummyData()
        {
            var matcher = DummyData.GetDummyMatches();
            var flaggor = DummyData.GetCountryFlags();
            anvandare = DummyData.GetDummyUsers();

            var matchViewModels = new List<dynamic>();
            foreach (var match in matcher)
            {
                matchViewModels.Add(new
                {
                    match.HemmalagId,
                    match.BortalagId,
                    Team1Flaggor = flaggor[match.HemmalagId],
                    Team2Flaggor = flaggor[match.BortalagId],
                    match.Id
                });
            }

            MatchBettingLista.ItemsSource = matchViewModels;            
        }
        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void MatchBettingLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var valdMatch = MatchBettingLista.SelectedItem;
            if (valdMatch != null)
            {
                VisaMatch.ItemsSource = new List<dynamic> { valdMatch };

                var matchId = (valdMatch as dynamic).Id;
                var filteredBets = anvandare
                    .SelectMany(a => a.Bets, (a, b) => new { Anvandare = a.Namn, Bet = b })
                    .Where(x => x.Bet.MatchId == matchId)
                    .Select(x => new
                    {
                        x.Anvandare,
                        x.Bet.GissningHemma,
                        x.Bet.GissningBorta
                    })
                    .ToList();

                SpelareBettingLista.ItemsSource = filteredBets;
            }
        }
    }
}

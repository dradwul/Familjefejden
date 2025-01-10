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
    public sealed partial class OverlayBetVy : Page
    {
        private List<Anvandare> spelare;
        private JsonService jsonService = new JsonService();
        private List<Bet> nyaBets = new List<Bet>();


        public OverlayBetVy()
        {
            this.InitializeComponent();
            HamtaInMatcher();
        }
        private async void HamtaInMatcher()
        {
            var matcher = await jsonService.HamtaAllaMatcherAsync();
            var flaggor = LagForemal.HamtaLagForemal();            

            var matchViewModels = new List<dynamic>();
            foreach (var match in matcher)
            {
                var hemmalagNamn = await jsonService.HamtaLagnamnFranLagId(match.HemmalagId);
                var bortalagNamn = await jsonService.HamtaLagnamnFranLagId(match.BortalagId);
                var hemmalagFlagga = flaggor.FirstOrDefault(l => l.Lag == hemmalagNamn)?.LagFlagga;
                var bortalagFlagga = flaggor.FirstOrDefault(l => l.Lag == bortalagNamn)?.LagFlagga;

                matchViewModels.Add(new
                {
                    HemmalagNamn = hemmalagNamn,
                    BortalagNamn = bortalagNamn,
                    HemmalagFlagga = hemmalagFlagga,
                    BortalagFlagga = bortalagFlagga,
                    match.Id
                });
            }

            MatchBettingLista.ItemsSource = matchViewModels;
        }
        private async void HamtaInSpelare()
        {
            spelare = await jsonService.HamtaAllaAnvandareAsync();
            SpelareBettingLista.ItemsSource = spelare.Select(s => new
            {
                Anvandare = s.Namn,
                GissningHemma = 0,
                GissningBorta = 0,
            }).ToList();
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
                HamtaInSpelare();
                MatchBettingLista.SelectedItem = null;
            }
        }

        private async void LaggBetKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var dataContext = button.DataContext as dynamic;
                if (dataContext != null)
                {
                    var anvandarNamn = dataContext.Anvandare;
                    var gissningHemmaTextBox = (button.Parent as Grid)?.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Margin.Left == 220);
                    var gissningBortaTextBox = (button.Parent as Grid)?.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Margin.Left == 350);

                    if (gissningHemmaTextBox != null && gissningBortaTextBox != null)
                    {
                        if (int.TryParse(gissningHemmaTextBox.Text, out int gissningHemma) && int.TryParse(gissningBortaTextBox.Text, out int gissningBorta))
                        {
                            var anvandare = spelare.FirstOrDefault(s => s.Namn == anvandarNamn);
                            if (anvandare != null)
                            {
                                var valdMatch = (VisaMatch.ItemsSource as IEnumerable<dynamic>)?.FirstOrDefault();
                                if (valdMatch != null)
                                {
                                    var existerandeBet = anvandare.Bets.FirstOrDefault(b => b.MatchId == valdMatch.Id);
                                    if (existerandeBet != null)
                                    {
                                        var dialog = new ContentDialog
                                        {
                                            Title = "Bet redan existerar",
                                            Content = "Du har redan placerat ett bet för denna match.",
                                            CloseButtonText = "OK"
                                        };
                                        await dialog.ShowAsync();
                                    }
                                    else
                                    {
                                        var nyttBet = new Bet
                                        {
                                            MatchId = valdMatch.Id,
                                            GissningHemma = gissningHemma,
                                            GissningBorta = gissningBorta
                                        };

                                        anvandare.Bets.Add(nyttBet);
                                        await jsonService.LaggTillBetAsync(anvandare.Id, nyttBet);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

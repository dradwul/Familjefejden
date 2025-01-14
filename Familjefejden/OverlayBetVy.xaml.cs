using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<Anvandare> allaSpelare;
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
                    Datum = match.Date.ToString("yyyy/MM/dd"),
                    Tid = match.Date.ToString("HH:mm"),
                    match.Id
                });
            }

            MatchBettingLista.ItemsSource = matchViewModels
                .OrderBy(m => m.Datum);
        }

        private async void HamtaInSpelare()
        {
            allaSpelare = await jsonService.HamtaAllaAnvandareAsync();
            if (allaSpelare.Count < 1)
            {
                HittadeIngaSpelareText.Visibility = Visibility.Visible;
                return;
            }

            HittadeIngaSpelareText.Visibility = Visibility.Collapsed;

            var valdMatch = (VisaMatch.ItemsSource as IEnumerable<dynamic>)?.FirstOrDefault();

            SpelareBettingLista.ItemsSource = allaSpelare.Select(s => new
            {
                Anvandare = s.Namn,
                GissningHemma = s.Bets.FirstOrDefault(b => b.MatchId == valdMatch.Id)?.GissningHemma ?? 0,
                GissningBorta = s.Bets.FirstOrDefault(b => b.MatchId == valdMatch.Id)?.GissningBorta ?? 0,
                HarExisterandeBet = s.Bets.Any(b => b.MatchId == valdMatch.Id)
            }).ToList();
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void SparaKnapp_Klickad(object sender, RoutedEventArgs e)
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
                            var anvandare = allaSpelare.FirstOrDefault(s => s.Namn == anvandarNamn);
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
                                            Title = "Bet existerar redan",
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

                                        button.IsEnabled = false;
                                        gissningHemmaTextBox.IsEnabled = false;
                                        gissningBortaTextBox.IsEnabled = false;
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

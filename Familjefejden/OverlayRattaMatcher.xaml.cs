using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Familjefejden.OverlaySpelschema;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Familjefejden
{
    public sealed partial class OverlayRattaMatcher : Page
    {
        JsonService jsonService = new JsonService();

        public OverlayRattaMatcher()
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
                // Skippa matcher som redan har resultat (är rättade)
                if (match.HemmalagMal != null || match.BortalagMal != null)
                    continue;

                // Skippa matcher som inte har spelats än
                if (match.Date > DateTime.Now)
                    continue;

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
                    match.Id,
                    Datum = match.Date.ToString("yyyy/MM/dd"),
                    Tid = match.Date.ToString("HH:mm"),
                    RattaHemmalag = string.Empty,
                    RattaBortalag = string.Empty
                });
            }
            // Sortera matcherna efter datum och tid
            matchViewModels = matchViewModels
               .OrderBy(m => DateTime.Parse($"{m.Datum} {m.Tid}"))
               .ToList();

            // Om inga matcher finns att visa, visa meddelande
            if (!matchViewModels.Any())
            {
                HittadeIngaSpelareText.Text = "Inga matcher att rätta just nu";
                HittadeIngaSpelareText.Visibility = Visibility.Visible;
            }
            else
            {
                HittadeIngaSpelareText.Visibility = Visibility.Collapsed;
            }

            RattaMatcherLista.ItemsSource = matchViewModels;

            // Visa den översta matchen i ComboBox
            if (matchViewModels.Any())
            {
                RattaMatcherLista.SelectedIndex = 0;
                VisaMatch.ItemsSource = new List<dynamic> { matchViewModels.First() };
                var matchId = matchViewModels.First().Id;
                var bets = await HamtaBetsForMatch(matchId);
                SpelarPoangPanel.ItemsSource = bets;
            }
        }

        private async Task<List<dynamic>> HamtaBetsForMatch(int matchId)
        {
            var spelareLista = await jsonService.HamtaAllaAnvandareAsync();
            var bets = new List<dynamic>();

            foreach (var spelare in spelareLista)
            {
                var bet = spelare.Bets.FirstOrDefault(b => b.MatchId == matchId);
                if (bet != null)
                {
                    bets.Add(new
                    {
                        Spelare = spelare.Namn,
                        GissningHemma = bet.GissningHemma,
                        GissningBorta = bet.GissningBorta
                    });
                }
            }

            return bets;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void RattaMatcherLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var valdMatch = RattaMatcherLista.SelectedItem as dynamic;
            if (valdMatch != null)
            {
                VisaMatch.ItemsSource = new List<dynamic> { valdMatch };                
                RattaMatcherLista.SelectedItem = null;

                var matchId = valdMatch.Id;
                var bets = await HamtaBetsForMatch(matchId);
                SpelarPoangPanel.ItemsSource = bets;

                RattaMatcherLista.SelectedItem = null;
            }
        }

        private async void RattaMatchKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var itemsSource = VisaMatch.ItemsSource as IEnumerable<dynamic>;
            var valdMatch = itemsSource?.FirstOrDefault();
            if (valdMatch != null)
            {
                // Hämta slutresultatet från TextBoxar
                var listViewItem = VisaMatch.ContainerFromItem(valdMatch) as ListViewItem;
                var hemmalagResultatTextBox = FindChild<TextBox>(listViewItem, "RattaHemmalag");
                var bortalagResultatTextBox = FindChild<TextBox>(listViewItem, "RattaBortalag");

                if (hemmalagResultatTextBox != null && bortalagResultatTextBox != null &&
                    int.TryParse(hemmalagResultatTextBox.Text, out int hemmalagResultat) &&
                    int.TryParse(bortalagResultatTextBox.Text, out int bortalagResultat))
                {
                    // Kontrollera om alla spelare har lagt sitt bet
                    var spelareBetLista = await jsonService.HamtaAllaAnvandareAsync();
                    var allaHarLagtBet = spelareBetLista.All(s => s.Bets.Any(b => b.MatchId == valdMatch.Id));

                    if (!allaHarLagtBet)
                    {
                        var dialogBet = new ContentDialog
                        {
                            Title = "Alla spelare har inte lagt sitt bet",
                            Content = "Du kan inte rätta matchen förrän alla spelare har lagt sitt bet.",
                            CloseButtonText = "Ok",
                            CornerRadius = new CornerRadius(10)
                        };
                        await dialogBet.ShowAsync();
                        return;
                    }

                    await jsonService.UppdateraMatchResultatAsync(valdMatch.Id, hemmalagResultat, bortalagResultat);

                    // Hämta alla spelare
                    var spelareLista = await jsonService.HamtaAllaAnvandareAsync();

                    foreach (var spelare in spelareLista)
                    {
                        var bet = spelare.Bets.FirstOrDefault(b => b.MatchId == valdMatch.Id);
                        if (bet != null)
                        {
                            int poang = 0;

                            // Rätt antal mål för hemmalag
                            if (bet.GissningHemma == hemmalagResultat)
                            {
                                poang += 1;
                            }

                            // Rätt antal mål för bortalag
                            if (bet.GissningBorta == bortalagResultat)
                            {
                                poang += 1;
                            }

                            // Rätt slutresultat
                            if (bet.GissningHemma == hemmalagResultat && bet.GissningBorta == bortalagResultat)
                            {
                                poang += 1;
                            }

                            // Uppdatera poäng för spelaren
                            await jsonService.UppdateraPoangForAnvandareAsync(spelare.Id, poang);
                        }
                    }

                    // Navigera tillbaka till huvudmenyn eller visa ett meddelande om att poängen har uppdaterats
                    var dialog = new ContentDialog
                    {
                        Title = "Poäng uppdaterade",
                        Content = "Poängen har uppdaterats för alla spelare.",
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                    Frame.Navigate(typeof(OverlayRattaMatcher));
                }
            }
        }

        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child == null) continue;

                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
    
}

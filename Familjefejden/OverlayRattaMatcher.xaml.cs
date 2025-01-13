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
                });
            }

            RattaMatcherLista.ItemsSource = matchViewModels;
        }
        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void RattaMatcherLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var valdMatch = RattaMatcherLista.SelectedItem;
            if (valdMatch != null)
            {
                VisaMatch.ItemsSource = new List<dynamic> { valdMatch };                
                RattaMatcherLista.SelectedItem = null;
            }
        }

        private void RattaMatchKnapp_Klickad(object sender, RoutedEventArgs e)
        {
         
        }
    }
}

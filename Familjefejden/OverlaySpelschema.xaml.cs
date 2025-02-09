﻿using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            this.Loaded += OverlaySpelschema_Loaded;

            tillagdaMatcher = new ObservableCollection<MatchForemal>();
            ListaSpelschema.ItemsSource = tillagdaMatcher;
        }

        private async void OverlaySpelschema_Loaded(object sender, RoutedEventArgs e)
        {
            var sparadeLag = await jsonService.HamtaAllaLagAsync();
            var ursprungligaLagForemal = LagForemal.HamtaLagForemal();

            var tillgangligaLag = ursprungligaLagForemal
                .Where(l => sparadeLag.Any(s => s.Namn == l.Lag))
                .ToList();

            Hemmalag.ItemsSource = tillgangligaLag;
            Bortalag.ItemsSource = tillgangligaLag;
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
                if (!MatchDag.SelectedDate.HasValue)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Välj datum",
                        Content = "Vänligen välj ett datum för matchen.",
                        CloseButtonText = "Ok",
                        CornerRadius = new CornerRadius(10)
                    };


                    await dialog.ShowAsync();
                    return;
                }
                if (valtLagHemma.Lag == valtLagBorta.Lag)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Ogiltigt val",
                        Content = "Hemmalag och bortalag kan inte vara samma lag.",
                        CloseButtonText = "Ok",
                        CornerRadius = new CornerRadius(10)
                    };

                    await dialog.ShowAsync();
                    return;
                }

                var datum = MatchDag.Date.DateTime.Date;
                var tid = MatchStart.Time;
                var matchDatum = new DateTime(datum.Year, datum.Month, datum.Day, tid.Hours, tid.Minutes, 0);

                int hemmalagId = await jsonService.HamtaLagIdFranNamn(valtLagHemma.Lag);
                int bortalagId = await jsonService.HamtaLagIdFranNamn(valtLagBorta.Lag);

                if(hemmalagId == -1 || bortalagId == -1)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Fel",
                        Content = "Ett av lagen kunde inte hittas.",
                        CloseButtonText = "Ok",
                        CornerRadius = new CornerRadius(10)
                    };
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
                    Datum = nyMatch.Date.ToString("yyyy/MM/dd"),
                    Tid = nyMatch.Date.ToString("HH:mm"),
                });

                Hemmalag.SelectedItem = null;
                Bortalag.SelectedItem = null;

                SparaKnapp.IsEnabled = true;
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "Välj lag",
                    Content = "Vänligen välj både hemmalag och bortalag.",
                    CloseButtonText = "Ok",
                    CornerRadius = new CornerRadius(10)
                };
                await dialog.ShowAsync();
                return;
            }
        }

        private async void SparaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            foreach(var match in listaMedMatcherSomSkaSparas)
            {
                bool laggaTillMatch = await jsonService.LaggTillMatchAsync(match);
                if (!laggaTillMatch)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Fel",
                        Content = "Matchen gick inte att lägga till.",
                        CloseButtonText = "Ok",
                        CornerRadius = new CornerRadius(10)
                    };

                    await dialog.ShowAsync();
                    return;
                }
            }

            Frame.Navigate(typeof(MainPage));
        }

        private async void TabortKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var knapp = sender as Button;
            var matchForemal = knapp.DataContext as MatchForemal;
            if (matchForemal == null) return;

            int hemmaLagID = await jsonService.HamtaLagIdFranNamn(matchForemal.HemmaLag);
            int bortaLagID = await jsonService.HamtaLagIdFranNamn(matchForemal.BortaLag);
            var matchAttTaBort = listaMedMatcherSomSkaSparas.FirstOrDefault(m => m.HemmalagId == hemmaLagID && m.BortalagId == bortaLagID && m.Date.ToString("yyyy/MM/dd") == matchForemal.Datum);
            
            if (matchAttTaBort != null)
            {
                listaMedMatcherSomSkaSparas.Remove(matchAttTaBort);
                tillagdaMatcher.Remove(matchForemal);
            }
        }

        public class MatchForemal
        {
            public string HemmaLagFlagga { get; set; }
            public string HemmaLag { get; set; }
            public string BortaLagFlagga { get; set; }
            public string BortaLag { get; set; }
            public string Datum { get; set; }
            public string Tid { get; set; }
        }
    }
}

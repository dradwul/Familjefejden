﻿using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                    Datum = match.Date.ToString("yyyy/MM/dd"),
                    Tid = match.Date.ToString("HH:mm")
                });
            }

            var kommande = matchObjekt
                .Select(m => new
                {
                    m.HemmalagNamn,
                    m.BortalagNamn,
                    m.HemmalagFlagga,
                    m.BortalagFlagga,
                    m.HemmalagMal,
                    m.BortalagMal,
                    m.Id,
                    m.MatchDatum,
                    m.Datum,
                    m.Tid,
                    SpelasJustNu = (DateTime.Now - m.MatchDatum).TotalHours >= 0 && (DateTime.Now - m.MatchDatum).TotalHours <= 2,
                    SpelasJustNuText = (DateTime.Now - m.MatchDatum).TotalHours >= 0 && (DateTime.Now - m.MatchDatum).TotalHours <= 2 ? "Spelas just nu!" : ""
                })
                .Where(m => m.MatchDatum >= DateTime.Today.Date)
                .OrderBy(m => m.MatchDatum);

            KommandeMatcher.ItemsSource = kommande.ToList();

            var resultat = matchObjekt
                .Where(m => m.MatchDatum < DateTime.Today.Date)
                .OrderByDescending(m => m.MatchDatum);

            ResultatMatcher.ItemsSource = resultat.ToList();
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

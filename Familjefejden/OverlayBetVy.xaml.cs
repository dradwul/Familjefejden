﻿using System;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverlayBetVy : Page
    {
        public OverlayBetVy()
        {
            this.InitializeComponent();
            LoadDummyData();
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        private void LoadDummyData()
        {
            var matches = DummyData.GetDummyMatches();
            var flags = DummyData.GetCountryFlags();

            var matchViewModels = new List<dynamic>();
            foreach (var match in matches)
            {
                matchViewModels.Add(new
                {
                    match.Team1,
                    match.Team2,
                    Team1Flag = flags[match.Team1],
                    Team2Flag = flags[match.Team2]
                });
            }

            MatchBettingLista.ItemsSource = matchViewModels;
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

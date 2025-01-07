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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverlayNyTurnering : Page
    {
        TurneringService turneringService = new TurneringService();
        JsonService jsonService = new JsonService();

        public OverlayNyTurnering()
        {
            this.InitializeComponent();
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void NastaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            // HÄMTA VÄRDE FRÅN COMBOBOX HÄR:
            // STATISKT VÄRDE ATM
            string turneringensNamn = "JVM";
            Turnering nyTurnering = turneringService.SkapaTurnering(turneringensNamn);
            jsonService.LaggaTillNyTurneringAsync(nyTurnering);

            Frame.Navigate(typeof(OverlayLaggaTillLag));
        }
    }
}

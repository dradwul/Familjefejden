using Familjefejden.Klasser;
using Familjefejden.Service;
using Klasser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Familjefejden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        JsonService jsonService = new JsonService();
        GruppService gruppService = new GruppService();
        TurneringService turneringService = new TurneringService();

        public MainPage()
        {
            this.InitializeComponent();
            HamtaDataAsync();
        }      

        private async void HamtaDataAsync()
        {
            await jsonService.KopieraFilTillLokalMappAsync("dataFil.json");
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



        // // //  TESTKNAPPAR:
        private async void TestaLaggaTillGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            Grupp nyGrupp = gruppService.SkapaGrupp("OOP2-Familjefejden");
            await jsonService.LaggTillNyGruppAsync(nyGrupp);
        }

        private async void TestaLaggaTillTurnering_Klickad(object sender, RoutedEventArgs e)
        {
            Turnering nyTurnering = turneringService.SkapaTurnering("JVM2025");
            await jsonService.LaggaTillNyTurneringAsync(nyTurnering);
        }

        private async void TestaLaggaTillAnvandare_Klickad(object sender, RoutedEventArgs e)
        {
            Anvandare nyAnvandare = gruppService.SkapaAnvandare("TestAnvändare");
            await jsonService.LaggTillAnvandareIGrupp(nyAnvandare);
        }

        private async void TestaLaggaTillLag_Klickad(object sender, RoutedEventArgs e)
        {
            Lag nyttLag = turneringService.SkapaLag("TestLag");
            await jsonService.LaggTillLagAsync(nyttLag);
        }

        private async void TestaLaggaTillMatch_Klickad(object sender, RoutedEventArgs e)
        {
            DateTime datum = DateTime.UtcNow;
            Random random = new Random();
            Match nyMatch = turneringService.SkapaMatch(datum, random.Next(0,10), random.Next(0, 10));
            await jsonService.LaggTillMatchAsync(nyMatch);
        }

        private async void TestaLaggaTillBet_Klickad(object sender, RoutedEventArgs e)
        {
            // Testar felhantering med slumpade IDn för användare och match
            Random random = new Random();
            int anvandareId = random.Next(0, 6);
            int matchId = random.Next(0, 10); 
            int hemmaMal = random.Next(0, 8);
            int bortaMal = random.Next(0, 8);
            Debug.WriteLine($"[anvId:{anvandareId}|matchId:{matchId}|resultat:{hemmaMal}-{bortaMal}]");
            Bet nyttBet = gruppService.LaggBetPaMatch(matchId, hemmaMal, bortaMal);
            await jsonService.LaggTillBetAsync(anvandareId, nyttBet);
        }
    }
}

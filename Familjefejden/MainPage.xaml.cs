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
        private void TestaMetoder_Klickad(object sender, RoutedEventArgs e)
        {
            List<Match> nyMatchlista = new List<Match>(turneringService.SkapaListaMedMatcher());
            Turnering nyTurnering = turneringService.NyTurnering(nyMatchlista);

            Grupp nyGrupp = gruppService.SkapaGrupp();
        }

        private async void TestLaggaTillGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            var nyGrupp = gruppService.SkapaGrupp();

            await jsonService.LaggaTillNyGruppAsync(nyGrupp);
            Debug.WriteLine("Ny grupp tillagd.");
        }

        private async void TestLaggaTillTurnering_Klickad(object sender, RoutedEventArgs e)
        {
            var nyMatchlista = turneringService.SkapaListaMedMatcher();
            var nyTurnering = turneringService.NyTurnering(nyMatchlista);

            await jsonService.LaggaTillNyTurneringAsync(nyTurnering);
            Debug.WriteLine("Ny turnering tillagd.");
        }
    }
}

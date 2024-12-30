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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Familjefejden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TopplistaService topplistaService = new TopplistaService();

        public MainPage()
        {
            this.InitializeComponent();
            InitTopplistaAsync();
        }      

        private async void InitTopplistaAsync()
        {
            await topplistaService.KopieraFilTillLokalMappAsync("topplista.json");
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


        // // // TESTTEST nedanför
        private void Testtest_Clicked(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            Topplista nyTopplista = new Topplista
            {
                Id = random.Next(15,15000),
                GruppId = random.Next(0,10),
                TurneringId = random.Next(0,10),
                AnvandarePoang = new Dictionary<string, int>
                {
                    {"Agda", 40 },
                    {"Körven", 33 },
                    {"Tjoffe", 10 },
                    {"Bulten", 9 },
                    {"x_DragoNsLayEr12_x", 40 }
                }
            };

            LaggTill(nyTopplista);
        }

        private async void LaggTill(Topplista nyTopplista)
        {
            await topplistaService.LaggTillNyTopplistaIJsonFilenAsync(nyTopplista);
        }
    }
}

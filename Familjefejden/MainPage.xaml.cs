using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
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
        private async void UppdateraTopplistaAsync()
        {
            await jsonService.UppdateraTopplistaAsync(5, 3, 2, new Dictionary<string, int> { { "Anvandare1", 10 } });
        }

        private void TestUppdateraTopplista_Klickad(object send, RoutedEventArgs e)
        {
            UppdateraTopplistaAsync();
        }

        private async void UppdateraAnvandareAsync()
        {
            await jsonService.UppdateraAnvandareAsync(10,"Agda",new List<Bet> { new Bet { Id = 1, GissningHemma = 2, GissningBorta = 3} });
        }

        private void TestUppdateraAnvandare_Klickad(object send, RoutedEventArgs e)
        {
            UppdateraAnvandareAsync();
        }
    }
}

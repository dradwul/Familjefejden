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
    public sealed partial class OverlayNyaSpelare : Page
    {
        GruppService gruppService = new GruppService();
        JsonService jsonService = new JsonService();
        private List<Anvandare> spelarLista = new List<Anvandare>();

        public OverlayNyaSpelare()
        {
            this.InitializeComponent();
            LaddaInBefintligaSpelare();
        }

        private async void LaddaInBefintligaSpelare()
        {
            spelarLista = await jsonService.HamtaAllaAnvandareAsync();
            SpelarListView.ItemsSource = spelarLista;
        }

        private async void NySpelare_Klickad(object sender, RoutedEventArgs e)
        {
            var nySpelare = NySpelare.Text.Trim();
            if (!string.IsNullOrEmpty(nySpelare))
            {
                var nySpelareAttSpara = gruppService.SkapaAnvandare(nySpelare);
                await jsonService.LaggTillAnvandareIGruppAsync(nySpelareAttSpara);

                spelarLista.Add(nySpelareAttSpara);
                SpelarListView.ItemsSource = null;
                SpelarListView.ItemsSource = spelarLista;

                NySpelare.Text = string.Empty;
            }
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyGrupp));
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

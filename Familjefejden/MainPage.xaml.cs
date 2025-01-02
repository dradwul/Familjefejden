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
    public sealed partial class MainPage : Page
    {        
        public MainPage()
        {
            this.InitializeComponent();
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

        private void NyGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyGrupp));
        }

        private void BetKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayBetVy));
        }
    }
}

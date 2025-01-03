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
    public sealed partial class OverlayNyGrupp : Page
    {
        public OverlayNyGrupp()
        {
            this.InitializeComponent();
        }

        private void NyGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            var nyGrupp = NyGrupp.Text;
            var valdTurnering = TurneringsLista.Text;
            if (!string.IsNullOrEmpty(nyGrupp) && !string.IsNullOrEmpty(valdTurnering))
            {
                GruppNamn.Text = $"{nyGrupp} har skapats för {valdTurnering}";
            }
            else
            {
                GruppNamn.Text = "Vänligen ange ett gruppnamn och välj en turnering";
            }
            NyGrupp.Text = string.Empty;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            //TODO: HANTERA SÅ MAN INTE KAN TRYCKA VIDARE INNAN MAN SKAPAT EN GRUPP OCH VALT TURNERING
            Frame.Navigate(typeof(OverlayNyaSpelare));
        }
    }
}

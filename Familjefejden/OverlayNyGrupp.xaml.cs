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
    public sealed partial class OverlayNyGrupp : Page
    {
        GruppService gruppService = new GruppService();
        JsonService jsonService = new JsonService();

        public OverlayNyGrupp()
        {
            this.InitializeComponent();
        }

        private void NyGrupp_Klickad(object sender, RoutedEventArgs e)
        {
            var nyGruppNamn = NyGrupp.Text;

            if (!string.IsNullOrEmpty(nyGruppNamn))
            {
                GruppNamn.Text = $"{nyGruppNamn} har skapats!";
                Grupp nyGrupp = gruppService.SkapaGrupp(nyGruppNamn);
                jsonService.LaggTillNyGruppAsync(nyGrupp);
                Frame.Navigate(typeof(OverlayNyaSpelare));
            }
            else
            {
                GruppNamn.Text = "Vänligen ange ett gruppnamn";
            }
            NyGrupp.Text = string.Empty;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void NyGrupp_SelectionChanged(object sender, RoutedEventArgs e)
        {
            LaggTillGruppKnapp.IsEnabled = !string.IsNullOrEmpty(NyGrupp.Text);
        }
    }
}

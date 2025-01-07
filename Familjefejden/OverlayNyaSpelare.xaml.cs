﻿using Familjefejden.Service;
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

        public OverlayNyaSpelare()
        {
            this.InitializeComponent();
        }

        private void NySpelare_Klickad(object sender, RoutedEventArgs e)
        {
            var nySpelare = NySpelare.Text;

            if (!string.IsNullOrEmpty(nySpelare))
            {
                Anvandare nySpelareAttSpara = gruppService.SkapaAnvandare(nySpelare);
                jsonService.LaggTillAnvandareIGruppAsync(nySpelareAttSpara);
                SpelarLista.Text += nySpelare + "\n";
                NySpelare.Text = String.Empty;
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

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
    public sealed partial class OverlaySpelschema : Page
    {
        public OverlaySpelschema()
        {
            this.InitializeComponent();
            this.Loaded += OverlaySpelschema_Loaded;
        }

        private void OverlaySpelschema_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var lagBild = new List<ImageItem>
            {
                new ImageItem { FlagBild = "ms-appx:///Assets/Canada.png", Text = "Kanada" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Czech_Republic.png", Text = "Tjeckien" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Finland.png", Text = "Finland" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Germany.png", Text = "Tyskland" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Kazakhstan.png", Text = "Kazakhstan" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Latvia.png", Text = "Lettland" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Slovakia.png", Text = "Slovakien" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Sweden.png", Text = "Sverige" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Switzerland.png", Text = "Schweiz" },
                new ImageItem { FlagBild = "ms-appx:///Assets/USA.png", Text = "USA" }
            };
            var lagBild2 = new List<ImageItem>
            {
                new ImageItem { FlagBild = "ms-appx:///Assets/Canada.png", Text = "Kanada" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Czech_Republic.png", Text = "Tjeckien" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Finland.png", Text = "Finland" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Germany.png", Text = "Tyskland" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Kazakhstan.png", Text = "Kazakhstan" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Latvia.png", Text = "Lettland" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Slovakia.png", Text = "Slovakien" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Sweden.png", Text = "Sverige" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Switzerland.png", Text = "Schweiz" },
                new ImageItem { FlagBild = "ms-appx:///Assets/USA.png", Text = "USA" }               
            };

            MatchLista.ItemsSource = lagBild;
            MatchLista2.ItemsSource = lagBild2;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyGrupp));
        }
    }

    public class ImageItem
    {
        public string FlagBild { get; set; }
        public string Text { get; set; }
    }
}

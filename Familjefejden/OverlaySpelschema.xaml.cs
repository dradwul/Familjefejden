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
            var items = new List<ImageItem>
            {
                new ImageItem { ImagePath = "ms-appx:///Assets/Canada.png", Text = "Kanada" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Czech_Republic.png", Text = "Tjeckien" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Finland.png", Text = "Finland" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Germany.png", Text = "Tyskland" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Kazakhstan.png", Text = "Kazakhstan" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Latvia.png", Text = "Lettland" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Slovakia.png", Text = "Slovakien" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Sweden.png", Text = "Sverige" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Switzerland.png", Text = "Schweiz" },
                new ImageItem { ImagePath = "ms-appx:///Assets/USA.png", Text = "USA" }
            };
            var items2 = new List<ImageItem>
            {
                new ImageItem { ImagePath = "ms-appx:///Assets/Canada.png", Text = "Kanada" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Czech_Republic.png", Text = "Tjeckien" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Finland.png", Text = "Finland" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Germany.png", Text = "Tyskland" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Kazakhstan.png", Text = "Kazakhstan" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Latvia.png", Text = "Lettland" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Slovakia.png", Text = "Slovakien" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Sweden.png", Text = "Sverige" },
                new ImageItem { ImagePath = "ms-appx:///Assets/Switzerland.png", Text = "Schweiz" },
                new ImageItem { ImagePath = "ms-appx:///Assets/USA.png", Text = "USA" }               
            };

            MatchLista.ItemsSource = items;
            MatchLista2.ItemsSource = items2;
        }
    }

    public class ImageItem
    {
        public string ImagePath { get; set; }
        public string Text { get; set; }
    }
}

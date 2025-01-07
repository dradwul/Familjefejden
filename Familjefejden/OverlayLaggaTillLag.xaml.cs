using Familjefejden.Klasser;
using Familjefejden.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class OverlayLaggaTillLag : Page
    {
        TurneringService turneringService = new TurneringService();
        JsonService jsonService = new JsonService();
        private ObservableCollection<LagItem> tillagdaLag;

        public OverlayLaggaTillLag()
        {
            this.InitializeComponent();
            this.Loaded += OverlayLaggaTillLag_Loaded;

            tillagdaLag = new ObservableCollection<LagItem>();
            TillagdaLag.ItemsSource = tillagdaLag;
        }

        private void OverlayLaggaTillLag_Loaded(object sender, RoutedEventArgs e)
        {
            var bildText = new List<ImageItem>
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
                new ImageItem { FlagBild = "ms-appx:///Assets/USA.png", Text = "USA" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Denmark.png", Text = "Danmark" },
                new ImageItem { FlagBild = "ms-appx:///Assets/Austria.png", Text = "Österrike" }
            };
            LagLista.ItemsSource = bildText;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyaSpelare));
        }

        private void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlaySpelschema));
        }

        private async void TillagdKnapp_KlickadAsync(object sender, RoutedEventArgs e)
        {
            var valtForemal = LagLista.SelectedItem as ImageItem;
            if (valtForemal != null)
            {
                if (tillagdaLag.Any(l => l.Lag == valtForemal.Text))
                {
                    var dialog = new MessageDialog("Laget är redan tillagt.");
                    await dialog.ShowAsync();
                }
                else if (tillagdaLag.Count >= 10)
                {
                    var dialog = new MessageDialog("Du kan inte lägga till fler än 10 lag.");
                    await dialog.ShowAsync();
                }
                else
                {
                    if (await jsonService.KontrolleraOmLagFinns(valtForemal.Text))
                    {
                        var dialog = new MessageDialog("Laget finns redan tillagt i databas.");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        Lag nyttLag = turneringService.SkapaLag(valtForemal.Text);
                        await jsonService.LaggTillLagAsync(nyttLag);
                        tillagdaLag.Add(new LagItem { LagFlagga = valtForemal.FlagBild, Lag = valtForemal.Text });
                    }
                }
            }
        }

        private void TabortKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var knapp = sender as Button;
            var lagForemal = knapp.DataContext as LagItem;
            if (lagForemal != null)
            {
                tillagdaLag.Remove(lagForemal);
            }
        }
    }

    public class LagItem
    {
        public string LagFlagga { get; set; }
        public string Lag { get; set; }
    }
}

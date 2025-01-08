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
        private ObservableCollection<LagForemal> tillagdaLag;

        public OverlayLaggaTillLag()
        {
            this.InitializeComponent();
            this.Loaded += OverlayLaggaTillLag_Loaded;

            tillagdaLag = new ObservableCollection<LagForemal>();
            TillagdaLag.ItemsSource = tillagdaLag;
        }

        private void OverlayLaggaTillLag_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var bildText = new List<LagForemal>
            {
                new LagForemal { LagFlagga = "ms-appx:///Assets/Canada.png", Lag = "Kanada" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Finland.png", Lag = "Finland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Czech_Republic.png", Lag = "Tjeckien" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Germany.png", Lag = "Tyskland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Kazakhstan.png", Lag = "Kazakhstan" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Latvia.png", Lag = "Lettland" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Slovakia.png", Lag = "Slovakien" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Sweden.png", Lag = "Sverige" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Switzerland.png", Lag = "Schweiz" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/USA.png", Lag = "USA" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Denmark.png", Lag = "Danmark" },
                new LagForemal { LagFlagga = "ms-appx:///Assets/Austria.png", Lag = "Österrike" }
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
            var valtForemal = LagLista.SelectedItem as LagForemal;
            if (valtForemal != null)
            {
                if (tillagdaLag.Any(l => l.Lag == valtForemal.Lag))
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
                    Lag nyttLag = turneringService.SkapaLag(valtForemal.Lag);
                    await jsonService.LaggTillLagAsync(nyttLag);
                    tillagdaLag.Add(new LagForemal { LagFlagga = valtForemal.LagFlagga, Lag = valtForemal.Lag });
                }
            }
        }

        private void TabortKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var knapp = sender as Button;
            var lagForemal = knapp.DataContext as LagForemal;
            if (lagForemal != null)
            {
                tillagdaLag.Remove(lagForemal);
            }
        }
    }
}

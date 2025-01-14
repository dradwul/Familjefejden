using Familjefejden.Klasser;
using Familjefejden.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Familjefejden
{
    public sealed partial class OverlayLaggaTillLag : Page
    {
        private readonly TurneringService turneringService = new TurneringService();
        private readonly JsonService jsonService = new JsonService();
        private readonly ObservableCollection<LagForemal> tillagdaLag;
        private List<Lag> befintligaLag = new List<Lag>();
        private List<Lag> nyaLagAttSpara = new List<Lag>();

        public OverlayLaggaTillLag()
        {
            this.InitializeComponent();
            this.Loaded += OverlayLaggaTillLag_Loaded;
            tillagdaLag = new ObservableCollection<LagForemal>();
            TillagdaLag.ItemsSource = tillagdaLag;
            UpdateNastaButtonState();
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayNyTurnering));
        }

        private async void OverlayLaggaTillLag_Loaded(object sender, RoutedEventArgs e)
        {
            var bildText = new List<LagForemal>(LagForemal.HamtaLagForemal());
            LagLista.ItemsSource = bildText;
            TurneringNamn.Text = await jsonService.HamtaTurneringsnamnFranId(1);
            await LaddaInBefintligaLag();
        }

        private async Task LaddaInBefintligaLag()
        {
            befintligaLag = await jsonService.HamtaAllaLagAsync();

            // Lägg till befintliga lag i UI-listan
            foreach (var lag in befintligaLag)
            {
                if (!tillagdaLag.Any(l => l.Lag == lag.Namn))
                {
                    var motsvarandeLagForemal = LagForemal.HamtaLagForemal()
                        .FirstOrDefault(l => l.Lag == lag.Namn);

                    if (motsvarandeLagForemal != null)
                    {
                        tillagdaLag.Add(new LagForemal
                        {
                            LagFlagga = motsvarandeLagForemal.LagFlagga,
                            Lag = lag.Namn
                        });
                    }
                }
            }

            UpdateNastaButtonState();
        }

        private async void TillagdKnapp_KlickadAsync(object sender, RoutedEventArgs e)
        {
            var valtForemal = LagLista.SelectedItem as LagForemal;
            if (valtForemal == null) return;

            if (tillagdaLag.Any(l => l.Lag == valtForemal.Lag))
            {
                var dialog = new ContentDialog
                {
                    Title = "Varning",
                    Content = "Laget är redan tillagt.",
                    CloseButtonText = "Ok",
                    CornerRadius = new CornerRadius(10)
                };
                await dialog.ShowAsync();
                return;
            }

            if (tillagdaLag.Count >= 10)
            {
                var dialog = new ContentDialog
                {
                    Title = "Varning",
                    Content = "Du kan inte lägga till fler än 10 lag.",
                    CloseButtonText = "Ok",
                    CornerRadius = new CornerRadius(10)
                };
                await dialog.ShowAsync();
                return;
            }

            var nyttLag = turneringService.SkapaLag(valtForemal.Lag);
            nyaLagAttSpara.Add(nyttLag);

            tillagdaLag.Add(new LagForemal
            {
                LagFlagga = valtForemal.LagFlagga,
                Lag = valtForemal.Lag
            });

            UpdateNastaButtonState();
            LagLista.SelectedItem = null;
        }

        private async void NastaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            // Spara alla nya lag innan vi går vidare
            foreach (var lag in nyaLagAttSpara)
            {
                await jsonService.LaggTillLagAsync(lag);
            }

            Frame.Navigate(typeof(OverlaySpelschema));
        }

        private async void TabortKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            var knapp = sender as Button;
            var lagForemal = knapp?.DataContext as LagForemal;
            if (lagForemal == null) return;

            // Ta bort från nyaLagAttSpara om det finns där
            var lagAttTaBort = nyaLagAttSpara.FirstOrDefault(l => l.Namn == lagForemal.Lag);

            if (lagAttTaBort != null)
            {
                nyaLagAttSpara.Remove(lagAttTaBort);
            }
            else
            {
                // Om laget inte finns i nyaLagAttSpara, ta bort från databasen
                var lagAttTaBortId = await jsonService.HamtaLagIdFranNamn(lagForemal.Lag);
                bool borttagen = await jsonService.TaBortLagAsync(lagAttTaBortId);
                if (!borttagen)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Fel",
                        Content = "Kunde inte ta bort laget. Försök igen.",
                        CloseButtonText = "Ok",
                        CornerRadius = new CornerRadius(10)
                    };
                    await dialog.ShowAsync();
                    return;
                }
            }

            tillagdaLag.Remove(lagForemal);
            UpdateNastaButtonState();
        }

        private void UpdateNastaButtonState()
        {
            NastaKnapp.IsEnabled = tillagdaLag.Count > 0;
        }

        private void LagLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LagLista.SelectedItem is LagForemal selectedItem)
            {
                LaggTillLagKnapp.IsEnabled = true;
            }
            else
            {
                LaggTillLagKnapp.IsEnabled = false;
            }
        }
    }
}
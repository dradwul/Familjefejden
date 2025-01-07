using Familjefejden.Klasser;
using Familjefejden.Service;
using Klasser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
    public sealed partial class OverlaySpelschema : Page
    {
        TurneringService turneringService = new TurneringService();
        JsonService jsonService = new JsonService();
        List<Match> matcherAttLaggaTill = new List<Match>();

        public OverlaySpelschema()
        {
            this.InitializeComponent();
            this.Loaded += OverlaySpelschema_Loaded;
            MatchDag.Date = DateTimeOffset.Now;
            MatchStart.Time = TimeSpan.Zero;
        }

        private void OverlaySpelschema_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Metod för att bara hämta lag som är med i turneringen
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

            Hemmalag.ItemsSource = lagBild;
            Bortalag.ItemsSource = lagBild2;
        }

        private void TillbakaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OverlayLaggaTillLag));
        }

        private async void LaggTillMatchKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            string hemmalagNamn = "";
            string bortalagNamn = "";
            DateTime matchTid = new DateTime();

            if (Hemmalag.SelectedItem is ImageItem hemmalagItem && Bortalag.SelectedItem is ImageItem bortalagItem)
            {
                hemmalagNamn = hemmalagItem.Text;
                bortalagNamn = bortalagItem.Text;

                matchTid = GetSelectedDateTime();
            }

            int hemmalagId = await jsonService.HamtaLagIdFranNamn(hemmalagNamn);
            int bortalagId = await jsonService.HamtaLagIdFranNamn(bortalagNamn);
            if(hemmalagId == bortalagId)
            {
                var dialog = new MessageDialog($"Vänligen ange olika lag");
                await dialog.ShowAsync();
                return;
            }
            if(hemmalagId == -1)
            {
                var dialog = new MessageDialog($"{hemmalagNamn} hittas inte i databasen.");
                await dialog.ShowAsync();
                return;
            }
            if (bortalagId == -1)
            {
                var dialog = new MessageDialog($"{bortalagNamn} hittas inte i databasen.");
                await dialog.ShowAsync();
                return;
            }

            Match nyMatch = turneringService.SkapaMatch(matchTid, hemmalagId, bortalagId);
            matcherAttLaggaTill.Add(nyMatch);

            // TODO: Lägg till i ListView
            // Provisorisk lösning
            var matchItem = new ListViewItem();
            matchItem.Content = $"{hemmalagNamn} - {bortalagNamn} {matchTid}";
            ListaSpelschema.Items.Add(matchItem);

            MatchDag.Date = DateTimeOffset.Now;
            MatchStart.Time = TimeSpan.Zero;
            Hemmalag.SelectedIndex = -1;
            Bortalag.SelectedIndex = -1;
        }

        private DateTime GetSelectedDateTime()
        {
            var datum = MatchDag.Date.DateTime;
            var tid = MatchStart.Time;

            return new DateTime(datum.Year, datum.Month, datum.Day, tid.Hours, tid.Minutes, 0, DateTimeKind.Utc);
        }

        private async void AccepteraKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            foreach(Match match in matcherAttLaggaTill)
            {
                await jsonService.LaggTillMatchAsync(match);
            }
            Frame.Navigate(typeof(MainPage));
        }
    }

    public class ImageItem
    {
        public string FlagBild { get; set; }
        public string Text { get; set; }
    }
}

using Familjefejden.Service;
using Klasser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.Mime.MediaTypeNames;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Familjefejden
{
    public sealed partial class OverlayTopplista : ContentDialog
    {
        private readonly JsonService service = new JsonService();

        public OverlayTopplista()
        {
            this.InitializeComponent();
            InitAsync();
        }

        private async Task InitAsync()
        {
            FormateraTillLista();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }


        private async void FormateraTillLista()
        {
            var gruppData = await service.HamtaSpecifikDataAsync("Grupp");

            if (gruppData is JObject gruppObjekt)
            {
                var anvandareLista = gruppObjekt["Anvandare"] as JArray;

                if (anvandareLista == null || !anvandareLista.Any())
                {
                    this.Title = "Inga användare i gruppen ännu.";
                    topplistaVy.Items.Clear();
                    return;
                }

                var sorteradLista = anvandareLista
                    .Select(a => new
                    {
                        Id = (int)a["Id"],
                        Namn = (string)a["Namn"],
                        TotalPoang = (int)a["TotalPoang"]
                    })
                    .OrderByDescending(a => a.TotalPoang);

                topplistaVy.Items.Clear();
                int placering = 1;

                this.Title = $"TOPPLISTA";

                // Skapa UI-element för varje användare
                foreach (var anvandare in sorteradLista)
                {
                    ListViewItem newListViewItem = new ListViewItem
                    {
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        FontSize = 14
                    };

                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    TextBlock textPlacering = new TextBlock
                    {
                        Text = $"{placering}.",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10, 0, 10, 0)
                    };
                    Grid.SetColumn(textPlacering, 0);

                    TextBlock textLeft = new TextBlock
                    {
                        Text = anvandare.Namn,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    Grid.SetColumn(textLeft, 1);

                    TextBlock textRight = new TextBlock
                    {
                        Text = anvandare.TotalPoang.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                    Grid.SetColumn(textRight, 2);

                    grid.Children.Add(textPlacering);
                    grid.Children.Add(textLeft);
                    grid.Children.Add(textRight);
                    newListViewItem.Content = grid;
                    topplistaVy.Items.Add(newListViewItem);

                    placering++;
                }
            }
        }

    }
}
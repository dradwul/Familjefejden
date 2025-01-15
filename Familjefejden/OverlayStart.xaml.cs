using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Familjefejden
{
    public sealed partial class OverlayStart : Page
    {
        public OverlayStart()
        {
            this.InitializeComponent();
            FadeInStoryboard.Begin();
        }

        private void SpelaKnapp_Klickad(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}

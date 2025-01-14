// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Familjefejden
{
    public class LagForemal
    {
        public string LagFlagga { get; set; }
        public string Lag { get; set; }

        public static List<LagForemal> HamtaLagForemal()
        {
            return new List<LagForemal>
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
        }
    }
}

using Klasser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Familjefejden
{
    public static class DummyData
    {
        /// <summary>
        /// DUMMY DATA SOM SKA TAS BORT
        /// </summary>
        public static List<Match> GetDummyMatches()
        {
            return new List<Match>
            {
                new Match { Id = 1, Team1 = "Sverige", Team2 = "Finland", Date = DateTime.Now.AddDays(1) },
                new Match { Id = 2, Team1 = "USA", Team2 = "Kanada", Date = DateTime.Now.AddDays(2) },
                new Match { Id = 3, Team1 = "Tjeckien", Team2 = "Slovakien", Date = DateTime.Now.AddDays(3) },
                new Match { Id = 1, Team1 = "Schweiz", Team2 = "Finland", Date = DateTime.Now.AddDays(1) },
                new Match { Id = 2, Team1 = "Lettland", Team2 = "Tyskland", Date = DateTime.Now.AddDays(2) },
                new Match { Id = 3, Team1 = "Kazakhstan", Team2 = "Sverige", Date = DateTime.Now.AddDays(3) },
                new Match { Id = 2, Team1 = "Lettland", Team2 = "USA", Date = DateTime.Now.AddDays(2) },
                new Match { Id = 3, Team1 = "Tjeckien", Team2 = "Sverige", Date = DateTime.Now.AddDays(3) },
                new Match { Id = 2, Team1 = "Kanada", Team2 = "Tyskland", Date = DateTime.Now.AddDays(2) }
            };
        }

        public static Dictionary<string, string> GetCountryFlags()
        {
            return new Dictionary<string, string>
            {
                { "Sverige", "ms-appx:///Assets/Sweden.png" },
                { "Finland", "ms-appx:///Assets/Finland.png" },
                { "USA", "ms-appx:///Assets/USA.png" },
                { "Kanada", "ms-appx:///Assets/Canada.png" },
                { "Tjeckien", "ms-appx:///Assets/Czech_Republic.png" },
                { "Slovakien", "ms-appx:///Assets/Slovakia.png" },
                { "Lettland", "ms-appx:///Assets/Latvia.png" },
                { "Tyskland", "ms-appx:///Assets/Germany.png" },
                { "Kazakhstan", "ms-appx:///Assets/Kazakhstan.png" },
                { "Schweiz", "ms-appx:///Assets/Switzerland.png" }
            };
        }
    }
}
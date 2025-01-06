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
                new Match { Id = 1, HemmalagId = 1, BortalagId = 2, Date = DateTime.Now.AddDays(1) },
                new Match { Id = 2, HemmalagId = 3, BortalagId = 4, Date = DateTime.Now.AddDays(2) },
                new Match { Id = 3, HemmalagId = 5, BortalagId = 6, Date = DateTime.Now.AddDays(3) },
                new Match { Id = 1, HemmalagId = 10, BortalagId = 2, Date = DateTime.Now.AddDays(1) },
                new Match { Id = 2, HemmalagId = 7, BortalagId = 8, Date = DateTime.Now.AddDays(2) },
                new Match { Id = 3, HemmalagId = 9, BortalagId = 1, Date = DateTime.Now.AddDays(3) },
                new Match { Id = 2, HemmalagId = 7, BortalagId = 3, Date = DateTime.Now.AddDays(2) },
                new Match { Id = 3, HemmalagId = 5, BortalagId = 1, Date = DateTime.Now.AddDays(3) },
                new Match { Id = 2, HemmalagId = 4, BortalagId = 8, Date = DateTime.Now.AddDays(2) }
            };
        }

        public static List<Match> GetFinishedMatches()
        {
            return new List<Match>
            {
                new Match { Id = 1, HemmalagId = 1, BortalagId = 2, HemmalagMal = 2, BortalagMal = 1, Date = DateTime.Now.AddDays(-1) },
                new Match { Id = 2, HemmalagId = 3, BortalagId = 4, HemmalagMal = 4, BortalagMal = 2, Date = DateTime.Now.AddDays(-2) },
                new Match { Id = 3, HemmalagId = 5, BortalagId = 6, HemmalagMal = 7, BortalagMal = 4, Date = DateTime.Now.AddDays(-3) },

            };
        }

        public static Dictionary<int, string> GetCountryFlags()
        {
            return new Dictionary<int, string>
            {
                { 1, "ms-appx:///Assets/Sweden.png" },
                { 2, "ms-appx:///Assets/Finland.png" },
                { 3, "ms-appx:///Assets/USA.png" },
                { 4, "ms-appx:///Assets/Canada.png" },
                { 5, "ms-appx:///Assets/Czech_Republic.png" },
                { 6, "ms-appx:///Assets/Slovakia.png" },
                { 7, "ms-appx:///Assets/Latvia.png" },
                { 8, "ms-appx:///Assets/Germany.png" },
                { 9, "ms-appx:///Assets/Kazakhstan.png" },
                { 10, "ms-appx:///Assets/Switzerland.png" }
            };
        }
    }
}
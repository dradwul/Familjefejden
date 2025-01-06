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
                new Match { Id = 1, HemmalagId = 1, BortalagId = 2, Date = new DateTime(2025, 12, 26, 19, 0, 0) },
                new Match { Id = 2, HemmalagId = 3, BortalagId = 4, Date = new DateTime(2025, 12, 26, 23, 30, 0) },
                new Match { Id = 3, HemmalagId = 5, BortalagId = 6, Date = new DateTime(2025, 12, 28, 19, 0, 0) },
                new Match { Id = 4, HemmalagId = 10, BortalagId = 2, Date = new DateTime(2025, 12, 29, 01, 30, 0) },
                new Match { Id = 2, HemmalagId = 7, BortalagId = 8, Date = new DateTime(2025, 12, 27, 19, 0, 0) },
                new Match { Id = 3, HemmalagId = 9, BortalagId = 1, Date = new DateTime(2025, 12, 27, 23, 0, 0) },
                new Match { Id = 2, HemmalagId = 7, BortalagId = 3, Date = new DateTime(2025, 12, 31, 19, 0, 0) },
                new Match { Id = 3, HemmalagId = 5, BortalagId = 1, Date = new DateTime(2025, 12, 31, 23, 0, 0) },
                new Match { Id = 2, HemmalagId = 4, BortalagId = 8, Date = new DateTime(2026, 1, 1, 19, 0, 0) }
            };
        }

        public static List<Match> GetFinishedMatches()
        {
            return new List<Match>
            {
                new Match { Id = 1, HemmalagId = 1, BortalagId = 2, HemmalagMal = 2, BortalagMal = 1, Date = new DateTime(2024, 12, 28, 19, 0, 0) },
                new Match { Id = 2, HemmalagId = 3, BortalagId = 4, HemmalagMal = 4, BortalagMal = 2, Date = new DateTime(2024, 12, 26, 19, 0, 0) },
                new Match { Id = 3, HemmalagId = 5, BortalagId = 6, HemmalagMal = 7, BortalagMal = 4, Date = new DateTime(2025, 1, 1, 19, 0, 0) },

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

        public static List<Anvandare> GetDummyUsers()
        {
            return new List<Anvandare>
    {
        new Anvandare
        {
            Id = 1,
            Namn = "Användare 1",
            Bets = new List<Bet>
            {
                new Bet { MatchId = 1, GissningHemma = 2, GissningBorta = 1 },
                new Bet { MatchId = 2, GissningHemma = 3, GissningBorta = 2 },
                new Bet { MatchId = 3, GissningHemma = 1, GissningBorta = 4 },
                new Bet { MatchId = 4, GissningHemma = 3, GissningBorta = 2 }
            },
            TotalPoang = 10,
            ArAdmin = false
        },
        new Anvandare
        {
            Id = 2,
            Namn = "Användare 2",
            Bets = new List<Bet>
            {
                new Bet { MatchId = 1, GissningHemma = 1, GissningBorta = 1 },
                new Bet { MatchId = 2, GissningHemma = 0, GissningBorta = 2 },
                new Bet { MatchId = 3, GissningHemma = 3, GissningBorta = 1 },
                new Bet { MatchId = 4, GissningHemma = 1, GissningBorta = 2 }
            },
            TotalPoang = 15,
            ArAdmin = true
        },
        new Anvandare
        {
            Id = 3,
            Namn = "Användare 3",
            Bets = new List<Bet>
            {
                new Bet { MatchId = 1, GissningHemma = 2, GissningBorta = 3 },
                new Bet { MatchId = 2, GissningHemma = 1, GissningBorta = 0 },
                new Bet { MatchId = 3, GissningHemma = 5, GissningBorta = 1 },
                new Bet { MatchId = 4, GissningHemma = 0, GissningBorta = 5 }
            },
            TotalPoang = 20,
            ArAdmin = false
        },
        new Anvandare
        {
            Id = 4,
            Namn = "Användare 4",
            Bets = new List<Bet>
            {
                new Bet { MatchId = 1, GissningHemma = 3, GissningBorta = 1 },
                new Bet { MatchId = 2, GissningHemma = 2, GissningBorta = 2 },
                new Bet { MatchId = 3, GissningHemma = 2, GissningBorta = 2 },
                new Bet { MatchId = 4, GissningHemma = 6, GissningBorta = 3 }
            },
            TotalPoang = 25,
            ArAdmin = false
        }
    };
        }
    }
}
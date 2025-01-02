using Klasser;
using System;
using System.Collections.Generic;

namespace Familjefejden.Service
{
    public class GruppService
    {
        int anvandareId = 1;
        string[] namnLista = new string[] { "Agda", "Brutus", "Olle", "Runken" };

        public Grupp SkapaGrupp()
        {
            List<Anvandare> medlemmar = new List<Anvandare>();
            for(int i = 0; i < namnLista.Length; i++)
            {
                medlemmar.Add(SkapaDummyAnvandare(namnLista[i]));    
            }

            return new Grupp
            {
                Id = 99,
                Namn = "Grupp99",
                Medlemmar = medlemmar
            };
        }

        public Anvandare SkapaDummyAnvandare(string namn)
        {
            Random random = new Random();

            var nyAnvandare = new Anvandare
            {
                Id = anvandareId,
                Namn = namn,
                Bets = { SkapaDummyBet(1), SkapaDummyBet(2), SkapaDummyBet(3) },
                TotalPoang = random.Next(10,30)
            };

            anvandareId++;
            return nyAnvandare;
        }

        public Bet SkapaDummyBet(int matchId)
        {
            Random random = new Random();
            return new Bet { MatchId = matchId, GissningHemma = random.Next(0, 5), GissningBorta = random.Next(0,5) };
        }
    }
}
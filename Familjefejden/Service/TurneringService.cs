using Familjefejden.Klasser;
using Klasser;
using System;
using System.Collections.Generic;

namespace Familjefejden.Service
{
    public class TurneringService
    {
        private string[] lagNamn = new string[] { "Sverige", "Kanada", "Tjeckien", "Tyskland", "USA", "Finland" };

        public Turnering NyTurnering(List<Match> matchLista)
        {
            return new Turnering
            {
                Id = 1,
                Namn = "MinTurnering1Lol",
                Lag = SkapaListaMedLag(),
                Matcher = matchLista
            };
        }

        public List<Match> SkapaListaMedMatcher()
        {
            Random random = new Random();
            List<Match> matchLista = new List<Match>();
            Stack<int> stack = new Stack<int>();
            stack.Push(2); stack.Push(4); stack.Push(5); stack.Push(3); stack.Push(6); stack.Push(1);
            for (int i = 0; i < 3; i++)
            {
                int lag1 = stack.Peek();
                stack.Pop();
                int lag2 = stack.Peek();
                stack.Pop();

                Match nyMatch = new Match
                {
                    Id = i+1,
                    Date = DateTime.UtcNow,
                    HemmalagId = lag1,
                    BortalagId = lag2,
                    HemmalagMal = random.Next(0,8),
                    BortalagMal = random.Next(0,8)
                };
                matchLista.Add(nyMatch);
            }
            return matchLista;
        }
        public List<Lag> SkapaListaMedLag()
        {
            List<Lag> lagLista = new List<Lag>();
            for(int i = 0; i < lagNamn.Length; i++)
            {
                Lag nyttLag = new Lag { Id = i+1, Name = lagNamn[i] };
                lagLista.Add(nyttLag);
            }
            return lagLista;
        }
    }
}

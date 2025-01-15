using Klasser;
using System.Collections.Generic;

namespace Familjefejden.Service
{
    public class GruppService
    {
        public Grupp SkapaGrupp(string namn)
        {
            return new Grupp
            {
                Id = 1, // TODO: Id-hantering. Låg prio iom att vi bara tänkt en turnering nu.
                Namn = namn,
                Medlemmar = new List<Anvandare>()
            };
        }

        public Anvandare SkapaAnvandare(string namn)
        {
            return new Anvandare
            {
                Namn = namn,
                TotalPoang = 0
            };
        }

        public Bet LaggBetPaMatch(int matchId,int gissningHemma, int gissningBorta)
        {
            return new Bet
            {
                MatchId = matchId,
                GissningHemma = gissningHemma,
                GissningBorta = gissningBorta
            };
        }
    }
}
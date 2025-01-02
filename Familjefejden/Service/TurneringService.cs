using Familjefejden.Klasser;
using Klasser;
using System;
using System.Collections.Generic;

namespace Familjefejden.Service
{
    public class TurneringService
    {
        public Turnering SkapaTurnering(string namn)
        {
            return new Turnering
            {
                Id = 1, // TODO: Id-hantering. Låg prio iom att vi bara tänkt en turnering nu.
                Namn = namn,
                Lag = new List<Lag>(),
                Matcher = new List<Match>()
            };
        }

        public Match SkapaMatch(DateTime datum, int hemmalagId, int bortalagId)
        {
            return new Match
            {
                Date = datum,
                HemmalagId = hemmalagId,
                BortalagId = bortalagId,
                HemmalagMal = null,
                BortalagMal = null
            };
        }

        public Lag SkapaLag(string namn)
        {
            return new Lag { Name = namn };
        }
    }
}

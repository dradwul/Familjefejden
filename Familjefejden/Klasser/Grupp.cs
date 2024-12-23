using System.Collections.Generic;

namespace SpelBet.Grupp
{
    public class Grupp
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public List<SpelBet.Anvandare.Anvandare> Medlemmar { get; set; }
    }
}

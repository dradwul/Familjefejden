using System.Collections.Generic;

namespace Klasser
{
    public class Anvandare
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public List<Bet> Bets { get; set; } = new List<Bet>();
        public int TotalPoang { get; set; }
        public bool? ArAdmin { get; set; }
    }
}
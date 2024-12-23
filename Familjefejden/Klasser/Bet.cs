namespace SpelBet.Bet
{
    public class Bet
    {
        public int Id { get; set; }
        public int AnvandareId { get; set; }
        public int MatchId { get; set; }
        public string Forutsagelse { get; set; }
        public int Poang { get; set; }
    }
}

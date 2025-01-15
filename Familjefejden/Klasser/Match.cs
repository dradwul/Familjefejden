using System;

namespace Klasser
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HemmalagId { get; set; }
        public int BortalagId { get; set; }
        public int? HemmalagMal { get; set; }
        public int? BortalagMal { get; set; }
    }
}
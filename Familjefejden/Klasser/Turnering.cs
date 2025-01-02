using Familjefejden.Klasser;
using System.Collections.Generic;

namespace Klasser
{
    public class Turnering
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public List<Lag> Lag { get; set; }
        public List<Match> Matcher { get; set; }
    }
}

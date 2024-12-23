using System.Collections.Generic;

namespace Klasser
{
    public class Turnering
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Match> Matches { get; set; }
    }
}

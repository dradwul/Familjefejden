using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Turnering.classes
{
    public class Turnering
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Match> Matches { get; set; }
    }
}

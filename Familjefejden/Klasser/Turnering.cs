using System.Collections.Generic;

namespace Klasser
{
    public class Turnering
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public List<int> Matcher { get; set; }
    }
}

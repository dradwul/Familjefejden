using System.Collections.Generic;

namespace Klasser
{
    public class Grupp
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public List<Anvandare> Medlemmar { get; set; }
        public List<int> TurneringarId { get; set; }
    }
}
using System.Collections.Generic;

namespace Klasser
{
    public class Grupp
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public List<int> MedlemmarId { get; set; }
        public Topplista Topplista { get; set; }
    }
}
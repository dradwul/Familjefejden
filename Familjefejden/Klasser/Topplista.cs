using System.Collections.Generic;

namespace Klasser
{
    /// <summary>
    /// JSON med en entitet av Topplista för varje turnering för varje grupp??
    /// </summary>
    public class Topplista
    {
        public int Id { get; set; }
        public int GruppId { get; set; }
        public Dictionary<Anvandare, int> AnvandarePoang { get; set; }
    }
}

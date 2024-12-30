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
        public int TurneringId { get; set; }
        public Dictionary<string, int> AnvandareIdPoang { get; set; } = new Dictionary<string, int>();
    }
}

using System.Collections.Generic;

namespace wrangler.models
{
    public class Initiative
    {
        public int Round { get; set; }
        public string Turn { get; set; }
        public List<Combatant> Combatants { get; set; }
    }
}

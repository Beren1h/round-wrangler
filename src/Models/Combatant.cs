using System.Collections.Generic;

namespace wrangler.models
{
    public class Combatant
    {
        public Combatant()
        {
            Affects = new List<Affect>();
        }

        public string Name { get; set; }
        public bool IsTurn { get; set; }
        public bool TurnTaken { get; set; }
        public bool IsActive { get; set; }
        public List<Affect> Affects { get; set; }
    }
}
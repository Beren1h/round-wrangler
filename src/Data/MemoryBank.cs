using System.Collections.Generic;
using wrangler.models;

namespace wrangler.data
{
    public class MemoryBank
    {
        public Affect AffectBeingAssigned { get; set; }
        public MemoryBank(){
            Affects = new List<Affect>();
            Combat = new Combat();
            Combatants = new List<Combatant>();
            Encounters = new List<Encounter>();
            InactiveReason = resources.Icons.DOWN;
        }

        public Combat Combat { get; set; }

        public List<Combatant> Combatants { get; set; }

        public List<Affect> Affects { get; set; }

        public List<Encounter> Encounters { get; set; }

        public int TensionDie { get; set; }

        public string InactiveReason { get; set; }

        public bool DeleteToggle { get; set; }
    }
}

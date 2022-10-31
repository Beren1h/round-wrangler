using System;
using System.Collections.Generic;
using wrangler.models;

namespace wrangler.events
{
    public class CombatantsChangedEventArgs : EventArgs
    {
        public List<Combatant> Combatants { get; set; }
    }

    public class CombatantChangedEventArgs : EventArgs
    {
        public Combatant Combatant { get; set; }
    }
}

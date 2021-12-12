using System;
using wrangler.models;

namespace wrangler.events
{
    public class CombatantChangedEventArgs : EventArgs
    {
        public Combatant Combatant { get; set; }
    }
}

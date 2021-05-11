using System.Collections.Generic;

namespace wrangler.models
{
    public class Combat
    {
        public int Round { get; set; }
        //public string Turn { get; set; }
        public Combatant Turn { get; set; }
    }
}

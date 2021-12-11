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
            InactiveReason = resources.Exits.DOWN;

            // var affect= new Affect { 
            //     Description = "hold person",
            //     Expiration = new Expiration {
            //         Round = 9
            //     },
            // };

            // var combatant = new Combatant {
            //     Name = "Fred",
            //     Affects = new List<Affect> {
            //         affect
            //     }
            // };

            //TensionDie = 4;
        }

        public Combat Combat { get; set; }

        public List<Combatant> Combatants { get; set; }

        public List<Affect> Affects { get; set; }

        public int TensionDie { get; set; }

        public string InactiveReason { get; set; }
    }
}

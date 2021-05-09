using System;
using System.Collections.Generic;
using System.Linq;
using wrangler.models;

namespace wrangler.data
{
    public class MemoryBank
    {
        public MemoryBank(){
            
            var affect= new Affect { 
                Id = Guid.NewGuid().ToString(),
                Description = "Fire",
                Expiration = new Expiration {
                    Turn = "Fred"
                },
                MetaData = new Dictionary<string, string> {
                    { resources.Affects.MetaDataKeys.CONCENTRATION, "Fred" }
                }
            };

            Combatants = new List<Combatant>{
                new Combatant { Name = "Fred", IsActive = true },
                new Combatant { 
                    Name = "Barney", 
                    IsActive = true, 
                    Affects = new List<Affect>{
                        affect
                    }  
                }
            };

            Combat = new Combat();
            
            Affects = new List<Affect>{
                affect
            };
        }

        public Combat Combat { get; set; }

        public List<Combatant> Combatants { get; set; }

        public List<Affect> Affects { get; set; }
    }
}

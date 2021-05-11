using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using wrangler.configuration;
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

            var affect= new Affect { 
                Description = "hold person",
                Expiration = new Expiration {
                    Round = 9
                },
            };

            var combatant = new Combatant {
                Name = "Fred",
                Affects = new List<Affect> {
                    affect
                }
            };

            // Affects.Add(affect);
            // Combatants.Add(combatant);

            // Combatants = new List<Combatant>{
            //     new Combatant { Name = "Fred", IsActive = true },
            //     new Combatant { 
            //         Name = "Barney", 
            //         IsActive = true, 
            //         Affects = new List<Affect>{
            //             affect
            //         }  
            //     }
            // };

            // Combat = new Combat();
            
            // Affects = new List<Affect>{
            //     affect
            // };
        }

        public Combat Combat { get; set; }

        public List<Combatant> Combatants { get; set; }

        public List<Affect> Affects { get; set; }
    }
}

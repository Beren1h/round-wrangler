using System;
using System.Linq;
using wrangler.data;
using wrangler.events;
using wrangler.models;

namespace wrangler.handlers
{
    public class CombatantHandler : Handler
    {
        private readonly MemoryBank _bank;

        public CombatantHandler(
            MemoryBank bank
        ){
            _bank = bank;
        }

        public event EventHandler<CombatantChangedEventArgs> OnCombatantChanged;

        public void RemoveAffectWidget(Combatant combatant, Affect affect)
        {
            combatant.Affects.Remove(affect);
            Notify();
        }

        public void AddAffectWidget(Combatant combatant, Affect affect)
        {
            combatant.Affects.Add(affect);

            var sorted = combatant.Affects.OrderBy(a => a.Description).ToList();

            combatant.Affects = sorted;

            Notify();
        }
        
        public void AddCombatant(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            
            if (_bank.Combatants.Any(c => c.Name == name))
            {
                return;
            }
            
            _bank.Combatants.Add(new Combatant {
                Name = name,
                IsActive = true
            });

            var sorted = _bank.Combatants.OrderBy(c => c.Name).ToList();

            _bank.Combatants = sorted;

            //Console.WriteLine($"{name} added");

            Notify();
        }

        public void RemoveCombatant(Combatant combatant)
        {
            //var match = _bank.Combatants.FirstOrDefault(c => c.Name == name);

            //match.IsActive = !match.IsActive;

            combatant.IsActive = !combatant.IsActive;
            
            //Console.WriteLine($"{combatant.Name} removed");

            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }

            Notify();
        }
    }
}
using System;
using System.Collections.Generic;
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
            
            var combtant = new Combatant {
                Name = name,
                IsActive = true,
                InactiveReason = resources.Exits.DOWN
            };

            _bank.Combatants.Add(combtant);

            var sorted = _bank.Combatants.OrderBy(c => c.Name).ToList();

            _bank.Combatants = sorted;

            Notify();
        }

        public void RemoveCombatant(Combatant combatant)
        {
            if (_bank.InactiveReason == resources.Exits.REMOVE)
            {
                var copy = new List<Combatant>(_bank.Combatants);
                foreach(var clone in copy)
                {
                    Console.WriteLine($"{clone.Name}, active={clone.IsActive}, reason={clone.InactiveReason}");
                    clone.IsTurn = false;
                }
                copy.Remove(combatant);
                var sorted = copy.OrderBy(c => c.Name).ToList();
                _bank.Combatants = sorted;
            }
            else
            {
                combatant.IsActive = !combatant.IsActive;
                combatant.InactiveReason = _bank.InactiveReason;
            }

            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }

            Notify();
        }
    }
}

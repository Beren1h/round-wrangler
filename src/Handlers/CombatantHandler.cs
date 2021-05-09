using System;
using System.Linq;
using wrangler.data;
using wrangler.models;

namespace wrangler.handlers
{
    public class CombatantHandler : Handler
    {
        private readonly MemoryBank _bank;
        public CombatantHandler(MemoryBank bank)
        {
            _bank = bank;
        }

        public void RemoveAffectWidget(Combatant combatant, Affect affect)
        {
            combatant.Affects.Remove(affect);
            Notify();
        }

        public void AddCombatant(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            
            _bank.Combatants.Add(new Combatant {
                Name = name,
                IsActive = true
            });

            var sorted = _bank.Combatants.OrderBy(c => c.Name).ToList();

            _bank.Combatants = sorted;

            Console.WriteLine($"{name} added");

            Notify();
        }

        public void RemoveCombatant(string name)
        {
            var match = _bank.Combatants.FirstOrDefault(c => c.Name == name);

            match.IsActive = !match.IsActive;

            Console.WriteLine($"{name} removed");

            Notify();
        }
    }
}
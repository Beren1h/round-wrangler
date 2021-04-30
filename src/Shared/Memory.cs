using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using wrangler.models;

namespace wrangler.handlers
{
    public class Memory
    {
        private readonly Party _party;
        public Memory(
            IOptions<Party> options
        ){
            _party = options.Value;

            Initiative = new Initiative{
                Combatants = new List<Combatant>()
            };
        }

        public Initiative Initiative { get; set; }

        private void IncludeParty()
        {
            foreach(var name in _party.Names)
            {
                AddCombatant(name);
            }
        }

        public void EndCombat()
        {
            Initiative.Round = 0;
            Initiative.Turn = string.Empty;
            Initiative.Combatants = new List<Combatant>();
            Notify();
        }

        public void NextRound()
        {
            if (Initiative.Round == 0)
            {
                IncludeParty();
            }

            foreach(var combatant in Initiative.Combatants.Where(c => c.IsActive))
            {
                combatant.TurnTaken = false;
            }

            Initiative.Turn = string.Empty;
            Initiative.Round++;

            Notify();
        }

        public void SetTurn(string name)
        {
            if (Initiative.Round == 0)
            {
                return;
            }

            if (!Initiative.Combatants.FirstOrDefault(c => c.Name == name).IsActive)
            {
                return;
            }

            foreach(var combatant in Initiative.Combatants.Where(c => c.Name == Initiative.Turn || c.Name == name))
            {
                combatant.TurnTaken = true;
            }

            Initiative.Turn = name;

            Notify();
        }

        public void RemoveCombatant(string name)
        {
            Initiative.Combatants.FirstOrDefault(c => c.Name == name).IsActive = false;

            Notify();
        }

        public void AddCombatant(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Initiative.Combatants.Add(new Combatant {
                Name = name,
                IsActive = true
            });

            var sorted = Initiative.Combatants.OrderBy(c => c.Name).ToList();

            Initiative.Combatants = sorted;

            Notify();
        }


        public event Action OnChange;
        private void Notify() => OnChange?.Invoke();
    }
}

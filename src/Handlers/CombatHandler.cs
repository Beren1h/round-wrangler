using System.Collections.Generic;
using System.Linq;
using wrangler.data;
using wrangler.models;

namespace wrangler.handlers
{
    public class CombatHandler : Handler
    {
        private readonly MemoryBank _bank;
        public CombatHandler(
            MemoryBank bank
        ){
            _bank = bank;
        }

        public void EndCombat()
        {
            _bank.Combat.Round = 0;
            _bank.Combat.Turn = string.Empty;
            _bank.Combatants = new List<Combatant>();
            Notify();
        }

        public void NextRound()
        {
            foreach(var combatant in _bank.Combatants.Where(c => c.IsActive))
            {
                combatant.IsTurn = false;
                combatant.TurnTaken = false;
            }

            _bank.Combat.Turn = string.Empty;
            _bank.Combat.Round++;

            Notify();
        }

        public void SetTurn(string name)
        {
            if (_bank.Combat.Round == 0)
            {
                return;
            }

            var current = _bank.Combatants.FirstOrDefault(c => c.Name == _bank.Combat.Turn);
            var incoming = _bank.Combatants.FirstOrDefault(c => c.Name == name);

            if (!incoming.IsActive)
            {
                return;
            }

            if (current != null)
            {
                current.TurnTaken = true;
                current.IsTurn = false;
            }

            _bank.Combat.Turn = name;
            incoming.TurnTaken = true;

            Notify();
        }
    }
}
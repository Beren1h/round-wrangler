using System;
using System.Collections.Generic;
using System.Linq;
using wrangler.data;
using wrangler.events;
using wrangler.models;

namespace wrangler.handlers
{
    public class CombatHandler : Handler
    {
        private readonly MemoryBank _bank;
        private readonly AffectHandler _affect;
        private readonly CombatantHandler _combatant;

        public CombatHandler(
            MemoryBank bank,
            AffectHandler affect,
            CombatantHandler combatant
        ){
            _bank = bank;
            _affect = affect;
            _combatant = combatant;
        }

        public event EventHandler<CombatantChangedEventArgs> OnCombatantChanged;

        public void EndCombat()
        {
            _bank.Combat.Round = 0;
            _bank.Combat.Turn = null;
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

            _bank.Combat.Turn = null;
            _bank.Combat.Round++;

            _affect.CheckExpirationOnRound();
            
            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }

            Notify();
        }

        public void SetTurn(Combatant incoming)
        {
            if (_bank.Combat.Round == 0)
            {
                return;
            }

            if (incoming.TurnTaken)
            {
                return;
            }
            
            var current = _bank.Combat.Turn;

            if (!incoming.IsActive)
            {
                return;
            }

            _affect.CheckExpirationOnTurn(incoming, current);

            if (current != null)
            {
                current.TurnTaken = true;
                current.IsTurn = false;
            }

            _bank.Combat.Turn = incoming;
            incoming.IsTurn = true;

            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }

            Notify();
        }
    }
}

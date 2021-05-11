using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using wrangler.configuration;
using wrangler.data;
using wrangler.events;
using wrangler.models;

namespace wrangler.handlers
{
    public class CombatHandler : Handler
    {
        private readonly MemoryBank _bank;
        private readonly AffectHandler _affect;
        private readonly Party _party;
        private readonly CombatantHandler _combatant;

        public CombatHandler(
            MemoryBank bank,
            AffectHandler affect,
            IOptions<Party> options,
            CombatantHandler combatant
        ){
            _party = options.Value;
            _bank = bank;
            _affect = affect;
            _combatant = combatant;
        }

        public event EventHandler<CombatantChangedEventArgs> OnCombatantChanged;

        public void EndCombat()
        {
            _bank.Combat.Round = 0;
            //_bank.Combat.Turn = string.Empty;
            _bank.Combat.Turn = null;
            _bank.Combatants = new List<Combatant>();
            Notify();
        }

        public void NextRound()
        {
            if (_bank.Combat.Round == 0)
            {
                foreach(var name in _party.Names)
                {
                    _combatant.AddCombatant(name);
                }
            }

            foreach(var combatant in _bank.Combatants.Where(c => c.IsActive))
            {
                combatant.IsTurn = false;
                combatant.TurnTaken = false;
            }

            // if (OnRoundChaning != null)
            // {
            //     var copy  _bank.Combat.Round;
            //     OnRoundChaning(this, new RoundChangingEventArgs{
            //         Incoming = copy++, 
            //         Outgoing = copy
            //     });
            // }

            //_bank.Combat.Turn = string.Empty;
            _bank.Combat.Turn = null;
            _bank.Combat.Round++;

            _affect.CheckExpirationOnRound();
            
            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }

            Notify();
        }

        // public event EventHandler<TurnChangingEventArgs> OnTurnChanging;
        // public event EventHandler<RoundChangingEventArgs> OnRoundChaning;

        // public event EventHandler<TurnChangedEventArgs> OnTurnChanged;
        // public event EventHandler<RoundChangedEventArgs> OnRoundChanged;

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
            
            //var current = _bank.Combatants.FirstOrDefault(c => c.Name == _bank.Combat.Turn);
            var current = _bank.Combat.Turn;
            //var incoming = _bank.Combatants.FirstOrDefault(c => c.Name == name);
            //var incoming = _bank.Combatants.FirstOrDefault(c => c == combatant);

            if (!incoming.IsActive)
            {
                return;
            }

            // if (OnTurnChanging != null)
            // {
            //     OnTurnChanging(this, new TurnChangingEventArgs{
            //         Incoming = incoming, 
            //         Outgoing = current
            //     });
            // }

            _affect.CheckExpirationOnTurn(incoming, current);

            if (current != null)
            {
                current.TurnTaken = true;
                current.IsTurn = false;
            }

            //_bank.Combat.Turn = name;
            _bank.Combat.Turn = incoming;
            //incoming.TurnTaken = true;
            incoming.IsTurn = true;

            // if (OnTurnChanged != null)
            // {
            //     OnTurnChanged(this, new TurnChangedEventArgs());
            // }

            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }

            Notify();
        }
    }
}

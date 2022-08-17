using System;
using System.Collections.Generic;
using System.Linq;
using wrangler.data;
using wrangler.events;
using wrangler.models;

namespace wrangler.handlers
{
    public class AffectRemovedArgs : EventArgs
    {
        public Affect Affect { get; set; }
    }

    public class AffectHandler : Handler
    {
        private readonly MemoryBank _bank;
        private readonly CombatantHandler _combatant;

        public AffectHandler(
            MemoryBank bank,
            CombatantHandler combatant
        ){
            _bank = bank;
            _combatant = combatant;
        }

        public event EventHandler<CombatantChangedEventArgs> OnCombatantChanged;
        public event EventHandler<AssignmentAffectChangedEventArgs> OnAssignmentAffectChanged;

        public void CheckExpirationOnRound()
        {
            var list = _bank.Affects.Select(a => a).ToList();

            foreach(var affect in _bank.Affects)
            {
                if (affect.Expiration.Round == _bank.Combat.Round &&
                    string.IsNullOrEmpty(affect.Expiration.Turn) &&
                    affect.Expiration.AtStart
                    )
                    {
                        list = Expire(list, affect);
                    }

                if (affect.Expiration.Round < _bank.Combat.Round)  
                {
                    list = Expire(list, affect);
                }
            }

            _bank.Affects = list;            
        }

        public void CheckExpirationOnTurn(Combatant incoming, Combatant current)
        {
            var list = _bank.Affects.Select(a => a).ToList();

            foreach(var affect in _bank.Affects)
            {
                if (affect.Expiration.Round == _bank.Combat.Round &&
                    affect.Expiration.Turn == incoming.Name &&
                    affect.Expiration.AtStart
                    )
                    {
                        list = Expire(list, affect);
                    }

                if (affect.Expiration.Round < _bank.Combat.Round || (
                    affect.Expiration.Round == _bank.Combat.Round &&
                    !string.IsNullOrEmpty(affect.Expiration.Turn) &&
                    affect.Expiration.Turn == current?.Name
                    ))
                    {
                        list = Expire(list, affect);
                    }
            }

            _bank.Affects = list;
        }

        private List<Affect> Expire(List<Affect> list, Affect affect)
        {
            foreach(var combatant in _bank.Combatants)
            {
                combatant.Affects.Remove(affect);
            }
            
            list.Remove(affect);

            UpdateConcentration(affect, false);

            return list;
        }

        public void AssignAffect(Affect affect)
        {
            if (_bank.AffectBeingAssigned == affect)
            {
                _bank.AffectBeingAssigned = null;
            }
            else
            {
                _bank.AffectBeingAssigned = affect;
            }
            
            if (OnAssignmentAffectChanged != null)
            {
                OnAssignmentAffectChanged(this, new AssignmentAffectChangedEventArgs{
                    Affect = _bank.AffectBeingAssigned
                });
            }
            
            Notify();
        }

        public event EventHandler<AffectRemovedArgs> AffectRemoved; 

        private void UpdateConcentration(Affect affect, bool adding)
        {
            if (affect.IsConcentration)
            {
                var combatant = _bank.Combatants.FirstOrDefault(c => c.Name == affect.Expiration.Turn);

                combatant.IsConcentrating = adding;
                combatant.ConcentrationAffect = adding ? affect : null;
            }

            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs());
            }
        }

        public void RemoveAffect(Affect affect)
        {
            _bank.Affects.Remove(affect);

            var sort = _bank.Affects.OrderBy(a => a.Description).ToList();

            _bank.Affects = sort;

            foreach(var combatant in _bank.Combatants)
            {
                _combatant.RemoveAffectWidget(combatant, affect);
            }

            if (AffectRemoved != null)
            {
                AffectRemoved(this, new AffectRemovedArgs { Affect = affect });
            }

            UpdateConcentration(affect, false);

            Notify();
        }

        public void AddAffect(Affect affect)
        {
            _bank.Affects.Add(affect);

            var sort = _bank.Affects.OrderBy(a => a.Description).ToList();

            _bank.Affects = sort;

            UpdateConcentration(affect, true);

            Notify();
        }
    }
}

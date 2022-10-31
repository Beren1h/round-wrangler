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
        public event EventHandler<CombatantsChangedEventArgs> OnCombatantsChanged;

        public void RemoveInactives()
        {
            var list = _bank.Combatants.Where(c => c.IsActive);

            _bank.Combatants = list.OrderBy(c => c.Name).ToList();

            if (OnCombatantsChanged != null)
            {
                OnCombatantsChanged(this, new CombatantsChangedEventArgs{
                    Combatants = _bank.Combatants
                });
            }

            Notify();            
        }

        public void ResetTurn(Combatant combatant)
        {
            if (combatant.IsActive && !combatant.IsTurn)
            {
                combatant.TurnTaken = false;
                
                if (OnCombatantChanged != null)
                {
                    OnCombatantChanged(this, new CombatantChangedEventArgs{
                        Combatant = combatant
                    });
                }

                Notify();
            }
        }

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
                InactiveReason = resources.Icons.SKULL
            };

            _bank.Combatants.Add(combtant);

            var sorted = _bank.Combatants.OrderBy(c => c.Name).ToList();

            _bank.Combatants = sorted;

            Notify();
        }

        public void RemoveCombatant(Combatant combatant)
        {
            if (_bank.DeleteToggle)
            {
                combatant.IsDeleted = true;
                _bank.DeleteToggle = false;
            }
            else
            {
                combatant.IsActive = !combatant.IsActive;
                combatant.InactiveReason = _bank.InactiveReason;
                combatant.IsConcentrating = false;
            }

            if (OnCombatantChanged != null)
            {
                OnCombatantChanged(this, new CombatantChangedEventArgs{
                    Combatant = combatant
                });
            }

            Notify();
        }
    }
}

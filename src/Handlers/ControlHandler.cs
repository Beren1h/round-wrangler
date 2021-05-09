using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using wrangler.data;
using wrangler.models;

namespace wrangler.handlers
{
    
    public class ControlHandler
    {
        private readonly MemoryBank _bank;
        private readonly CombatantHandler _combatant;

        public ControlHandler(
            MemoryBank bank,
            CombatantHandler combatant
        ){
            _bank = bank;
            _combatant = combatant;
        }

        public string AddedCombatantName {get; set; }

        public List<WidgetProperties> GetAffectWidgets(Combatant combatant)
        {
            var list = new List<WidgetProperties>();
            
            foreach(var affect in combatant.Affects)
            {
                list.Add(new WidgetProperties{
                    Text = affect.Description,
                    Theme = "red",
                    RemoveClick = () => _combatant.RemoveAffectWidget(combatant, affect)
                });
            }

            return list;            
        }

        public string DetermineConcentrationCssClass()
        {
            var combatant = _bank.Combatants.FirstOrDefault(c => c.Name == _bank.Combat.Turn);

            if (_bank.Combat.Round == 0 || combatant == null)
            {
                return string.Empty;
            }

            foreach(var affect in _bank.Affects)
            {
                if (affect.MetaData.ContainsKey(resources.Affects.MetaDataKeys.CONCENTRATION) && 
                    affect.MetaData[resources.Affects.MetaDataKeys.CONCENTRATION] == combatant.Name)
                {
                    return "concentration";
                }
            }

            return string.Empty;
        }

        public string DetermineCombatantWidgetCssClass(Combatant combatant)
        {
            if (!combatant.IsActive)
            {
                return "not-active";
            }

            if (combatant.IsActive && (combatant.IsTurn || combatant.TurnTaken))
            {
                return "turn-taken";
            }

            return string.Empty;
        }

        public void OnEnter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                _combatant.AddCombatant(AddedCombatantName);
                AddedCombatantName = string.Empty;
            }
        }
    }
}
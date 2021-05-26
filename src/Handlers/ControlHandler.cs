using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using wrangler.configuration;
using wrangler.data;
using wrangler.models;

namespace wrangler.handlers
{
    
    public class ControlHandler
    {
        private readonly MemoryBank _bank;
        private readonly CombatantHandler _combatant;
        private readonly CombatHandler _combat;


        public ControlHandler(
            MemoryBank bank,
            CombatantHandler combatant,
            CombatHandler combat,
            IOptions<Party> options
        ){
            _bank = bank;
            _combatant = combatant;
            _combat = combat;
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

        public void DetermineCombatantWidgetBodyClickAction(Combatant combatant)
        {
            if (_bank.AffectBeingAssigned != null)
            {
                _combatant.AddAffectWidget(combatant, _bank.AffectBeingAssigned);
            }
            else
            {
                //_combat.SetTurn(combatant.Name);
                _combat.SetTurn(combatant);
            }
        }

        public string DetermineAffectCssClass(Affect affect)
        {
            if (_bank.AffectBeingAssigned == affect)
            {
                return "assignment";
            }

            return string.Empty;
        }

        public string DetermineConcentrationCssClass()
        {
            //var combatant = _bank.Combatants.FirstOrDefault(c => c.Name == _bank.Combat.Turn);
            var combatant = _bank.Combat.Turn;

            if (_bank.Combat.Round == 0 || combatant == null)
            {
                return string.Empty;
            }

            foreach(var affect in _bank.Affects)
            {
                if (affect.IsConcentration == resources.Affects.Concentration.YES && affect.Expiration.Turn == combatant.Name)
                {
                    return "concentration";
                }
                // if (affect.MetaData.ContainsKey(resources.Affects.MetaDataKeys.CONCENTRATION) && 
                //     affect.MetaData[resources.Affects.MetaDataKeys.CONCENTRATION] == combatant.Name)
                // {
                //     return "concentration";
                // }
            }

            return string.Empty;
        }

        public string DetermineCombatantWidgetCssClass(Combatant combatant)
        {
            // if (!combatant.IsActive)
            // {
            //     return "not-active";
            // }

            // if (combatant.IsActive && (combatant.IsTurn || combatant.TurnTaken))
            // {
            //     return "turn-taken";
            // }

            var css = string.Empty;

            if (!combatant.IsActive || combatant.IsTurn || combatant.TurnTaken)
            {
                css += " muted";
            }

            if (combatant.IsActive && 
                _bank.Affects.Any(a => a.IsConcentration == resources.Affects.Concentration.YES && 
                a.Expiration.Turn == combatant.Name))
            {
                css += " concentration";
            }

            // if (combatant.IsActive && combatant.IsTurn)
            // {
            //     css += " turn";
            // }

            return css;
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
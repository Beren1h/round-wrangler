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

            Affects = new List<Affect>();
        }

        public Initiative Initiative { get; set; }

        public List<Affect> Affects { get; set; }

        public Affect SelectedAffect { get; set; }

        public string TurnAffects { get; set; }

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

        public List<Affect> Expired(List<Affect> list, Affect affect)
        {
            foreach(var combatant in Initiative.Combatants)
            {
                combatant.Affects.Remove(affect);
            }
            
            list.Remove(affect);

            return list;
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
            
            var list = Affects.Select(a => a).ToList();

            foreach(var affect in Affects)
            {
                if (affect.Expiration.Round <= Initiative.Round &&
                    string.IsNullOrEmpty(affect.Expiration.Turn) &&
                    affect.Expiration.Pointer == resources.Affects.Pointers.START)
                    {
                        list = Expired(list, affect);
                    }

                if (affect.Expiration.Round < Initiative.Round &&
                    string.IsNullOrEmpty(affect.Expiration.Turn) &&
                    affect.Expiration.Pointer == resources.Affects.Pointers.END)
                    {
                        list = Expired(list, affect);
                    }
            }

            Affects = list;
            // foreach(var affect in Effects)
            // {
            //     if (affect.Expiration.Round >= Initiative.Round && affect.Expiration.Pointer == resources.Effects.Pointers.START)
            //     {

            //     }
            // }


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

            var list = Affects.Select(a => a).ToList();

            foreach(var affect in Affects)
            {
                if (affect.Expiration.Round == Initiative.Round &&
                    affect.Expiration.Turn == name &&
                    affect.Expiration.Pointer == resources.Affects.Pointers.START)
                    {
                        list = Expired(list, affect);
                    }

                if (affect.Expiration.Round == Initiative.Round &&
                    affect.Expiration.Turn == Initiative.Turn &&
                    affect.Expiration.Pointer == resources.Affects.Pointers.END)
                    {
                        list = Expired(list, affect);
                    }
            }

            Affects = list;

            Initiative.Turn = name;

            var combatant2 = Initiative.Combatants.FirstOrDefault(c => c.Name == name);

            foreach (var affect in combatant2.Affects)
            {
                TurnAffects += affect.Description;
            }

            Notify();
        }

        public void RemoveCombatant(string name)
        {
            //Console.WriteLine("name");
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

        public void RemoveAffect(Affect affect)
        {
            Affects.Remove(affect);

            foreach(var combatant in Initiative.Combatants)
            {
                combatant.Affects.Remove(affect);
            }

            Notify();
        }
        public void AddAffect(AffectSubmit submit)
        {
            var effect = new Affect();

            effect.Id = Guid.NewGuid().ToString();
            effect.Expiration = submit.Expiration;
            effect.Description = submit.Description;

            if(submit.IsConcentration)
            {
                effect.MetaData.Add("concentration", submit.Expiration.Turn);
            }

            Affects.Add(effect);
            Notify();
        }

        public void ApplyAffect(Combatant combatant)
        {
            //var combatant = Initiative.Combatants.FirstOrDefault(c => c.Name == name);

            combatant.Affects.Add(SelectedAffect);

            Notify();
        }

        public void RemoveAffect(Affect affect, Combatant combatant)
        {
            Console.WriteLine($"remove affect: {affect.Description}");
            //var affect = Affects.FirstOrDefault(a => a.Id == id);
            Initiative.Combatants.FirstOrDefault(c => c.Name == combatant.Name).Affects.Remove(affect);
            Notify();
        }

        public event Action OnChange;
        private void Notify() => OnChange?.Invoke();
    }
}

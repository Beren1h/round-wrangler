using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using wrangler.data;
using wrangler.models;

namespace wrangler.handlers
{
    public class EncounterHandler : Handler
    {
        private readonly MemoryBank _bank;
        private readonly CombatantHandler _combatant;
        private readonly CombatHandler _combat;
        public EncounterHandler(
            MemoryBank bank,
            CombatantHandler combatant,
            CombatHandler combat
        ){
            _bank = bank;
            _combat = combat;
            _combatant = combatant;
        }

        public async Task UploadEncounters(IBrowserFile file)
        {
            try
            {
                var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                var json = System.Text.Encoding.ASCII.GetString(ms.ToArray());

                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                };

                var upload = JsonSerializer.Deserialize<FileUpload>(json, options);

                _bank.Encounters = upload.Encounters;

                Console.WriteLine(upload.Session);

                Notify();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateEncounter(string name)
        {
            var encounter = _bank.Encounters.FirstOrDefault(e => e.Name == name);

            foreach(var combatant in encounter.Combatants)
            {
                _combatant.AddCombatant(combatant);
            }

            Notify();
        }
    }
}

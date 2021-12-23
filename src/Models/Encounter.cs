using System.Collections.Generic;

namespace wrangler.models
{
    public class FileUpload
    {
        public string Session { get; set; }
        public List<Encounter> Encounters { get; set; }
    }
    public class Encounter
    {
        public string Name { get; set; }
        public List<string> Combatants { get; set; }
    }
}

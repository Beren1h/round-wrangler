using System.Collections.Generic;

namespace wrangler.models
{
    public class AffectSubmit : Affect
    {
        public bool IsConcentration { get; set; }
    }

    public class Affect
    {
        public Affect()
        {
            Expiration = new Expiration();
            MetaData = new Dictionary<string, string>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public bool AssignmentMode { get; set; }
        public Expiration Expiration { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
    }

    public class Expiration
    {
        public int Round { get; set; }
        public string Turn { get; set; }
        public string Pointer { get; set; }
    }
}

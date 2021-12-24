using System;

namespace wrangler.models
{
    public class Affect
    {
        private string _id;
        public Affect()
        {
            _id = Guid.NewGuid().ToString();
            Expiration = new Expiration();
        }

        public string Id { get { return _id; } }
        public string Description { get; set; }
        public bool AssignmentMode { get; set; }
        public Expiration Expiration { get; set; }
        public bool IsConcentration { get; set; }
    }

    public class Expiration
    {
        public int Round { get; set; }
        public string Turn { get; set; }
        public bool AtStart { get; set; }
    }
}

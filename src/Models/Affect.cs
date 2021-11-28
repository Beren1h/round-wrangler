using System;
using System.Collections.Generic;

namespace wrangler.models
{
    // public class AffectSubmit : Affect
    // {
    //     public bool IsConcentration { get; set; }
    // }

    public class Affect
    {
        private string _id;
        public Affect()
        {
            _id = Guid.NewGuid().ToString();
            IsConcentration = resources.Affects.Concentration.NO;
            Expiration = new Expiration{
                Pointer = resources.Affects.Pointers.END
            };
        }

        public string Id { get { return _id; } }
        public string Description { get; set; }
        public bool AssignmentMode { get; set; }
        public Expiration Expiration { get; set; }
        public string IsConcentration { get; set; }
    }

    public class Expiration
    {
        public int Round { get; set; }
        public string Turn { get; set; }
        public string Pointer { get; set; }
    }
}

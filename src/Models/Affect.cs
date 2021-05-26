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
            //this.Id = Guid.NewGuid().ToString();
            _id = Guid.NewGuid().ToString();
            IsConcentration = resources.Affects.Concentration.NO;
            Expiration = new Expiration{
                Pointer = resources.Affects.Pointers.END
            };
            // MetaData = new Dictionary<string, string>{
            //     { resources.Affects.MetaDataKeys.CONCENTRATION, "false" }
            // };
        }

        public string Id { get { return _id; } }
        public string Description { get; set; }
        public bool AssignmentMode { get; set; }
        public Expiration Expiration { get; set; }
        //public bool IsConcentration { get; set; }

        public string IsConcentration { get; set; }

        //public Dictionary<string, string> MetaData { get; set; }
    }

    public class Expiration
    {
        public int Round { get; set; }
        public string Turn { get; set; }
        public string Pointer { get; set; }
    }
}

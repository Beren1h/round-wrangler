using System;
using wrangler.models;

namespace wrangler.events
{
    public class AssignmentAffectChangedEventArgs : EventArgs
    {
        public Affect Affect { get; set; }
    }
}

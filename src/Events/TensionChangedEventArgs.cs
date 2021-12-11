using System;

namespace wrangler.events
{
    public class TensionChangedEventArgs : EventArgs
    {
        public int Tension { get; set; }
    }
}

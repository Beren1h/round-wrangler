using System;
using wrangler.data;
using wrangler.events;

namespace wrangler.handlers
{
    public class TensionHandler : Handler
    {
        private readonly MemoryBank _bank;
        public TensionHandler(
            MemoryBank bank
        ){
            _bank = bank;
        }

        public event EventHandler<TensionChangedEventArgs> OnTensionChanged;

        public void Add()
        {
            _bank.TensionDie++;
            if (OnTensionChanged != null)
            {
                OnTensionChanged(this, new TensionChangedEventArgs{ Tension = _bank.TensionDie });
            }

            Notify();
        }

        public void Clear()
        {
            _bank.TensionDie = 0;
            if (OnTensionChanged != null)
            {
                OnTensionChanged(this, new TensionChangedEventArgs{ Tension = _bank.TensionDie });
            }

            Notify();            
        }
    }
}

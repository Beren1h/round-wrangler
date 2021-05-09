using System;

namespace wrangler.handlers
{
    public class Handler
    {
        public event Action OnChange;
        public void Notify() => OnChange?.Invoke();  
    }
}

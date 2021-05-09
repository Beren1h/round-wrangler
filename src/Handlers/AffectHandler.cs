using System;
using System.Linq;
using wrangler.data;
using wrangler.models;
using static wrangler.resources.Affects;

namespace wrangler.handlers
{
    public class AffectHandler : Handler
    {
        private readonly MemoryBank _bank;

        public AffectHandler(
            MemoryBank bank
        ){
            _bank = bank;
        }

        public void AddAffect(AffectSubmit submit)
        {
            var affect = new Affect();

            affect.Id = Guid.NewGuid().ToString();
            affect.Description = submit.Description;
            affect.Expiration = submit.Expiration;

            if (submit.IsConcentration == "true")
            {
                affect.MetaData.Add(MetaDataKeys.CONCENTRATION, submit.Expiration.Turn);
            }
            
            _bank.Affects.Add(affect);

            var sort = _bank.Affects.OrderBy(a => a.Description).ToList();

            _bank.Affects = sort;

            Notify();
        }
    }
}

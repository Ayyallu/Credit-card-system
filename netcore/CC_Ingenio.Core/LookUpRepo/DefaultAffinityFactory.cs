using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class DefaultAffinityFactory
    {
        public static DefaultAffinity CreateDefaultAffinityFactory(string ACode, string ADescription)
        {
            DefaultAffinity objDefaultAffinity = new DefaultAffinity()
            {
                DefaultAffinityCode = ACode,
                Description = ADescription
            };
            return objDefaultAffinity;

        }
    }
}

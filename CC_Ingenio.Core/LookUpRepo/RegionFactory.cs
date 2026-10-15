using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class RegionFactory
    {
        public static Region CreateRegionLookUpFactory(string ACode, string ADescription)
        {
            Region objRegion = new Region()
            {
                RegionCode = ACode,
                RegionName = ADescription
            };
            return objRegion;

        }
    }
}

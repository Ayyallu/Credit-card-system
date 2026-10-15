using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class SpokeFactory
    {
        public static Spoke CreateSpokeLookUpFactory(string ACode, string ADescription)
        {
            Spoke objSpoke = new Spoke()
            {
                SpokeCode = ACode,
                SpokeName = ADescription
            };
            return objSpoke;
        }
    }
}

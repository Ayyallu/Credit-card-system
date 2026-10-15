using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class CommunaFactory
    {
        public static Communa CreateCommunaLookUpFactory(string ACode, string ADescription)
        {
            Communa objCommuna = new Communa()
            {
                CommunaCode = ACode,
                CommunaName = ADescription
            };
            return objCommuna;
        }
    }
}

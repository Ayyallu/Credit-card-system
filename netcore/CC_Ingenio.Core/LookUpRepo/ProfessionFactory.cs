using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ProfessionFactory
    {
        public static Profession CreateProfessionFactory(string ACode, string ADescription)
        {
            Profession objProfession = new Profession()
            {
                ProfessionCode = ACode,
                Description = ADescription
            };
            return objProfession;

        }
    }
}

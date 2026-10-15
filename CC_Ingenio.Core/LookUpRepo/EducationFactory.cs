using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class EducationFactory
    {
        public static Education CreateEducationFactory(string ACode, string ADescription)
        {
            Education objEducation = new Education()
            {
                EducationCode = ACode,
                Description = ADescription
            };
            return objEducation;

        }
    }
}

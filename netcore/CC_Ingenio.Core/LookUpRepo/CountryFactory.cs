using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class CountryFactory
    {

        
        public static Country CreateCountryLookUpFactory(string ACode, string ADescription)
        {
            Country objCountry = new Country()
            {
                CountryCode = ACode,
                CountryName = ADescription
            };
            return objCountry;

        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class CityCategoryFactory
    {
        public static CityCategory CreateCityCategoryLookUpFactory(string ACode, string ADescription)
        {
            CityCategory objCityCategory = new CityCategory()
            {
                CityCategoryCode = ACode,
                Description = ADescription
            };
            return objCityCategory;

        }
    }
}

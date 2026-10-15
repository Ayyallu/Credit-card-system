using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ProductClassFactory
    {
        public static ProductClass CreateProductClassFactory(string ACode, string ADescription)
        {
            ProductClass objProductClass = new ProductClass()
            {
                ProductClassCode = ACode,
                Description = ADescription
            };
            return objProductClass;

        }
    }
}

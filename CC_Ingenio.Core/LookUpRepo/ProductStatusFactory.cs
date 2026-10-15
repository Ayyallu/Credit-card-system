using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ProductStatusFactory
    {
        public static ProductStatus CreateProductStatusFactory(string ACode, string ADescription)
        {
            ProductStatus objProductStatus = new ProductStatus()
            {
                ProductStatusCode = ACode,
                Description = ADescription
            };
            return objProductStatus;

        }
    }
}

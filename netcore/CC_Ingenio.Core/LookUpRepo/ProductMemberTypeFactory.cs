using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ProductMemberTypeFactory
    {
        public static ProductMemberType CreateProductMemberTypeFactory(string ACode, string ADescription)
        {
            ProductMemberType objProductMemberType = new ProductMemberType()
            {
                ProductMemberTypeCode = ACode,
                Description = ADescription
            };
            return objProductMemberType;

        }
    }
}

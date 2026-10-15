using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ProductSubCategoryFactory
    {
        public static ProductSubCategory CreateProductSubCategoryFactory(string ACode, string ADescription)
        {
            ProductSubCategory objProductSubCategory = new ProductSubCategory()
            {
                ProductSubCategoryCode = ACode,
                Description = ADescription
            };
            return objProductSubCategory;

        }
    }
}

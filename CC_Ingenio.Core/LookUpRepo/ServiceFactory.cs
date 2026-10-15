using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ServiceFactory
    {
        public static Service CreateServiceFactory(string ACode, string ADescription)
        {
            Service objService = new Service()
            {
                ServiceCode = ACode,
                Description = ADescription
            };
            return objService;

        }
    }
}

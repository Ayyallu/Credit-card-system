using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class ContractTypeFactory
    {
        public static ContractType CreateContractTypeLookUpFactory(string ACode, string ADescription)
        {
            ContractType objContractType = new ContractType()
            {
                ContractTypeCode = ACode,
                Description = ADescription
            };
            return objContractType;
        }
    }
}

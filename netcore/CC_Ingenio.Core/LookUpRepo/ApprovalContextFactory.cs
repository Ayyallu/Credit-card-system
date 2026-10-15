using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
   public class ApprovalContextFactory
    {
       public static ApprovalContext CreateCityCategoryLookUpFactory(string AContextCode, string AContextDescription, string AResonCode)
        {
            ApprovalContext objApprovalContext = new ApprovalContext()
            {
                ReasonContextCode = AContextCode,
                ReasonContextDescription = AContextDescription,
                ReasonCode = AResonCode
            };
            return objApprovalContext;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class MemberTypeFactory
    {
        public static MemberType CreateMemberType(string ACode, string ADescription)
        {
            MemberType objMemberType = new MemberType()
            {
                MemberTypeCode = ACode,
                MemberTypeName = ADescription
            };
            return objMemberType;

        }
    }
}

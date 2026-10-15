using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class AlertTypeFactory
    {
        public static AlertType CreateAlertType(string AAlertCode, string AAlertName)
        {
            AlertType objAlertType = new AlertType()
            {
                AlertCode = AAlertCode,
                AlertName = AAlertName
            };
            return objAlertType;
        }
    }
}

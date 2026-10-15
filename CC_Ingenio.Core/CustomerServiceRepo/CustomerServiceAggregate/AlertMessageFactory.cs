using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class AlertMessageFactory
    {
        public static AlertMessage CreateAlertMessage(int AAlertId, string ACardNumber, string AAlertType,
           string AMessage, bool AIsMobileAlert, bool AIsEmailAlert, string AMobileNo, string AEmailId, bool AIsCharged)
        {
            AlertMessage objAlertMessage = new AlertMessage()
            {
                AlertId = AAlertId,
                CardNumber = ACardNumber,
                AlertType = AAlertType,
                Message = AMessage,
                IsMobileAlert = AIsMobileAlert,
                IsEmailAlert = AIsEmailAlert,
                MobileNo = AMobileNo,
                EmailId = AEmailId,
                IsCharged = AIsCharged
            };
            return objAlertMessage;
        }
    }
}

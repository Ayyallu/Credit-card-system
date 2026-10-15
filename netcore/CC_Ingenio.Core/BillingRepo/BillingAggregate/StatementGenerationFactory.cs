using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.BillingRepo.BillingAggregate
{
    public class StatementGenerationFactory
    {
        public static StatementGeneration CreateStatementGeneration(string ACardNumber, string ACustomerCardName, DateTime ABillDate, 
                                int? ABillMonth, int? ABillYear,
                                decimal AOpeningRevolvingBalance, decimal AClosingRevolvingBalance, DateTime APaymentDueDate ) 
        {
            StatementGeneration objStatementGeneration = new StatementGeneration ()
            {
                CardNumber = ACardNumber,
                CustomerCardName = ACustomerCardName,
                BillDate = ABillDate,
                BillMonth = ABillMonth,
                BillYear = ABillYear,
                OpeningRevolvingBalance = AOpeningRevolvingBalance,
                ClosingRevolvingBalance = AClosingRevolvingBalance,
                PaymentDueDate = APaymentDueDate
            };

            return objStatementGeneration;
        }
    }
}

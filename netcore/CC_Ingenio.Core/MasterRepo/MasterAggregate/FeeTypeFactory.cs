using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class FeeTypeFactory
    {
        public static FeeType CreateFeeType(string AFeeCode,string AFeeTypeCode, decimal AFeeAmount, decimal AMonthsFrom, string AReasonCode,
            decimal AFirstYearAmount, decimal ARenwalFee1, decimal ARenwalFee2, decimal ARenwalFee3, decimal ARenwalFee4, decimal ARenwalFee5, 
            decimal ARenwalFee6, string AAdjustCode, string APeriod, decimal ADiscountRatePercent, decimal AFlatFee, decimal AMinFee, bool AFlag,string AOrganizationCode)
        {
            FeeType objFeeType = new FeeType()
            {
                FeeCode = AFeeCode,
                FeeTypeCode = AFeeTypeCode,
                FeeAmount = AFeeAmount,
                MonthsFrom = AMonthsFrom,
                ReasonCode = AReasonCode,
                FirstYearAmount = AFirstYearAmount,
                RenwalFee1 = ARenwalFee1,
                RenwalFee2 = ARenwalFee2,
                RenwalFee3 = ARenwalFee3,
                RenwalFee4 = ARenwalFee4,
                RenwalFee5 = ARenwalFee5,
                RenwalFee6 = ARenwalFee6,
                AdjustCode = AAdjustCode,
                Period = APeriod,
                DiscountRatePercent = ADiscountRatePercent,
                FlatFee = AFlatFee,
                MinFee = AMinFee,
                Flag = AFlag,
                OrganizationCode = AOrganizationCode 
            };

            return objFeeType;
        }
    }
}

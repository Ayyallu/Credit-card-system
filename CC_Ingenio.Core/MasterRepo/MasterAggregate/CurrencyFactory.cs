using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class CurrencyFactory
    {
        public static Currency CreateCurrency(string ACurrencyCode, string ACurrencyName, decimal APrecision, decimal? AUFCLPRate, string ACountryCode,
            bool AIsBlackListed, bool AIsActive, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode,string AApprovedBy = null )
        {
            Currency objCurrency = new Currency()
            {
                CurrencyCode = ACurrencyCode,
                OrganizationCode = AOrganizationCode ,
                CurrencyName = ACurrencyName,
                Precision = APrecision,
                UFCLPRate = AUFCLPRate,
                CountryCode = ACountryCode,
                IsActive = AIsActive,
                IsBlackListed = AIsBlackListed,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate, 
                ApprovedOn =null,
                ApprovalStatus = AApprovalStatus

            };


            return objCurrency;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class UFRateSettingsFactory
    {
        public static UFRateSettings CreateUFRateSettings( string AOrganizationCode, DateTime AEffectiveDate, decimal AUFRate,
                string ACreatedBy, DateTime ACreatedOn, DateTime ALastUpdatedOn, string ALastUpdatedBy ) 
        {
            UFRateSettings objUFRateSettings = new UFRateSettings()
            {
                OrganizationCode = AOrganizationCode,
                EffectiveDate = AEffectiveDate,
                UFRate = AUFRate,

                CreatedBy = ACreatedBy,
                CreatedOn = ACreatedOn,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = ALastUpdatedOn
            };
            return objUFRateSettings;
        }
    }
}

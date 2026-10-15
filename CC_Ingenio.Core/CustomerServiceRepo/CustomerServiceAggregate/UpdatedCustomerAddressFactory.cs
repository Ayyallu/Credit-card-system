using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
   public class UpdatedCustomerAddressFactory
    {
        public static UpdatedCustomerAddress CreateUpdatedCustomerAddress(int ACustomerId,string ACardNumber, string AAddressTypeCode, string ATypeOfResidenceClearance,
           string ACountryCode,string ARegionCode, string ACityCode, string ACommunaCode, string AStreet, string ANumber, string ABlock, string AComplementOfAddress,
           string ATown, string ALandmark, string APostalCode, string APOBox, string APhone, string AMobile, string AFax,string AUpdatedBy,DateTime AUpdatedOn,
            string AApprovedBY,DateTime AApprovedOn, string AApprovalStatus)
        {            
            UpdatedCustomerAddress objCustomerAddress = new UpdatedCustomerAddress()
            {
                CustomerId = ACustomerId,
                CardNumber = ACardNumber,
                AddressTypeCode = AAddressTypeCode,
                TypeOfResidenceClearance = ATypeOfResidenceClearance,
                CountryCode = ACountryCode,
                RegionCode = ARegionCode,
                CityCode = ACityCode,
                CommunaCode = ACommunaCode,
                Street = AStreet,
                Number = ANumber,
                Block = ABlock,
                ComplementOfAddress = AComplementOfAddress,
                Town = ATown,
                Landmark = ALandmark,
                PostalCode = APostalCode,
                POBox = APOBox,
                Phone = APhone,
                Mobile = AMobile,
                Fax = AFax,
                LastUpdatedBy = AUpdatedBy,
                LastUpdatedOn = AUpdatedOn,
                ApprovedBy = AApprovedBY,
                ApprovedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,//need to change. Date should be provided after approval
                ApprovalStatus = AApprovalStatus
            };
            return objCustomerAddress;
        }
    }
}

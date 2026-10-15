using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerAddressFactory 
    {
        public static CustomerAddress CreateCustomerAddress(int AAddressId, string AAddressTypeCode, int ACustomerId, string ATypeOfResidenceClearance,
            string ARegionCode, string ACityCode, string ACommunaCode, string AStreet, string ANumber, string ABlock, string AComplementOfAddress,
            string ATown, string ALandmark, string APostalCode, string APOBox, string APhone, string AMobile, string AFax)
        {
            CustomerAddress objCustomerAddress = new CustomerAddress() { 
                AddressId = AAddressId,
                AddressTypeCode = AAddressTypeCode,
                CustomerId = ACustomerId,
                TypeOfResidenceClearance = ATypeOfResidenceClearance,
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
                Fax = AFax           
            };
            return objCustomerAddress;
        }
    }
}

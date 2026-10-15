using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CC_Ingenio.Core.ContactRepo.AddressAggregate
{
    public static class AddressFactory
    {
        public static Address CreateAddress(string ALine1, string ALine2,
            string ACity, string AState, string ACountry)
        {
            Address objAddress = new Address();
            objAddress.Line1 = ALine1;
            objAddress.Line2 = ALine2;
            objAddress.City = ACity;
            objAddress.State = AState;
            objAddress.Country = ACountry;

            return objAddress;

        
        }
    }
}

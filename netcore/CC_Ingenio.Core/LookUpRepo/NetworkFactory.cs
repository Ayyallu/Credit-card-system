using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public static class NetworkFactory
    {
        public static Network CreateNetwork(string ANetworkCode, string ADescription)
        {
            Network objNetwork = new Network() 
            {
                NetworkCode = ANetworkCode,
                Description = ADescription
            };
            return objNetwork;
        }
    }
}

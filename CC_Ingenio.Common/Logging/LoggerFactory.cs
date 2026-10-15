using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Common.Logging
{
    public static class LoggerFactory
    {
        static ILoggerFactory ilFactory = null;

        public static void SetCurrent(ILoggerFactory ALogFactory)
        {
            ilFactory = ALogFactory;
        }


        public static ILogger CreateLog()
        {
            return (ilFactory != null) ? ilFactory.Create() :null;
        }

        
    }


}

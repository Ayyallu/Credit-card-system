using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Common.Logging
{
    public class HandleLogFactory : ILoggerFactory
    {
        public ILogger Logger { get; set; }
        public ILogger Create()
        {
            HandleLog log = new HandleLog();
            Logger = log;
            return log;
        }

        
        
    }
}

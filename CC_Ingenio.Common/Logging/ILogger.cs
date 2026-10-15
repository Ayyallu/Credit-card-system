using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace CC_Ingenio.Common.Logging
{
    public interface ILogger
    {
        
        void LogError(String AMessage, int AEventId, int APriority); 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
namespace CC_Ingenio.Common.Logging
{
    public class HandleLog:ILogger
    {
        public LogEntry leLogEntry;
        LogWriterFactory lgLogWriterFactory;
        IConfigurationSource icSoure;
        DateTime serverTime = DateTime.Now;
        public HandleLog()
        {
                icSoure = ConfigurationSourceFactory.Create();
                leLogEntry = new LogEntry();
                lgLogWriterFactory = new LogWriterFactory(icSoure);

            
                Logger.SetLogWriter(lgLogWriterFactory.Create(),false);
              
            
        }


        public void LogError(String AMessage, int AEventId, int APriority)
        {
            leLogEntry.Message = AMessage;
            leLogEntry.EventId = AEventId;
            leLogEntry.Priority = APriority;
            leLogEntry.TimeStamp = serverTime;
            Logger.Write(leLogEntry);
        }


        public void LogError(Exception AMessage, int AEventId, int APriority)
        {
            leLogEntry.Message = AMessage.StackTrace;
            leLogEntry.EventId = AEventId;
            leLogEntry.Priority = APriority;
            leLogEntry.TimeStamp = serverTime;
            Logger.Write(leLogEntry);
        }
        
    }
}

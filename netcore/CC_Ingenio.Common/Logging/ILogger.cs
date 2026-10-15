using System;

namespace CC_Ingenio.Common.Logging
{
    public interface ILogger
    {

        void LogError(String AMessage, int AEventId, int APriority);
    }
}

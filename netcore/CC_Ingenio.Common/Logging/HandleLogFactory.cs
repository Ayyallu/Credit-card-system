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

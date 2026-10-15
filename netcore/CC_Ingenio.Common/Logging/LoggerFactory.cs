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
            return (ilFactory != null) ? ilFactory.Create() : null;
        }


    }


}

using System;
using MSLogging = Microsoft.Extensions.Logging;

namespace CC_Ingenio.Common.Logging
{
    /// <summary>
    /// Temporary net8 replacement for the legacy Enterprise Library 6.0
    /// backed HandleLog (see PORT_NOTES.md). Enterprise Library was
    /// discontinued around 2013 and has no .NET 8 support, so this keeps
    /// the same class name/shape (HandleLog : ILogger) and the same
    /// (string/Exception, eventId, priority) call pattern callers already
    /// use, but writes through Microsoft.Extensions.Logging's console
    /// provider instead. This is NOT a production logging configuration -
    /// whoever owns the real ASP.NET Core host (once CC_Ingenio.Web is
    /// ported) should replace this with a logger obtained from the app's
    /// actual DI container/configuration (file, Application Insights, etc.)
    /// instead of the standalone console logger created here.
    /// </summary>
    public class HandleLog : ILogger
    {
        private static readonly MSLogging.ILoggerFactory FactoryInstance =
            MSLogging.LoggerFactory.Create(builder => builder.AddConsole());

        private readonly MSLogging.ILogger _logger =
            FactoryInstance.CreateLogger("CC_Ingenio");

        public void LogError(String AMessage, int AEventId, int APriority)
        {
            _logger.LogError("[EventId={EventId}, Priority={Priority}] {Message}", AEventId, APriority, AMessage);
        }

        public void LogError(Exception AMessage, int AEventId, int APriority)
        {
            _logger.LogError(AMessage, "[EventId={EventId}, Priority={Priority}] {Message}", AEventId, APriority, AMessage.StackTrace);
        }
    }
}

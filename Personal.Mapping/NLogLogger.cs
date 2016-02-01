using System;
using NLog;

namespace Personal.Mapping
{
    public class NLogLogger : CostEffectiveCode.Common.ILogger
    {
        private readonly Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Fatal(Exception exception)
        {
            _logger.Fatal(exception);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void ErrorFormat(string message, params object[] arguments)
        {
            _logger.Debug(message, arguments);
        }

        public void DebugFormat(string message, params object[] arguments)
        {
            _logger.Debug(message, arguments);
        }

        public void Error(string unexpextedError, Exception exception)
        {
            _logger.Error(exception, unexpextedError);
        }
    }
}

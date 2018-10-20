using System;
using Abstractions;
using Microsoft.Extensions.Logging;

namespace Plugins.Host.AspNetCore
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }
        public void LogError(Exception exception, string message)
        {
            _logger.LogError(exception, message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
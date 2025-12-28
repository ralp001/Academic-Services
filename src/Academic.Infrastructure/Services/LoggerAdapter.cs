using Academic.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Academic.Infrastructure.Services
{
    // Concrete implementation of our logger contract using Microsoft's ILogger
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            // The LoggerFactory creates the correct, typed logger instance
            _logger = loggerFactory.CreateLogger<T>();
        }

        // --- Core Implementations ---

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            // Used for unexpected but non-critical events (e.g., a field was missing but defaulted safely)
            _logger.LogWarning(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            // Used for critical failures that halt execution (e.g., database connection lost)
            _logger.LogError(ex, message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            // Used for detailed, verbose logging during development or deep debugging
            _logger.LogDebug(message, args);
        }
    }
}
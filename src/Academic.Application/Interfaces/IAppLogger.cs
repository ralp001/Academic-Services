using System;

namespace Academic.Application.Interfaces
{
    // Abstraction for logging, independent of any specific logging technology
    public interface IAppLogger<T>
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
        void LogDebug(string message, params object[] args);
    }
}